import os
import json
from typing import Dict, Any, List, Optional
from datetime import datetime

import requests
import pandas as pd
import streamlit as st


def get_base_url() -> str:
    """
    URL base del servicio RAG (FastAPI).
    Se puede configurar con la variable de entorno RAG_BASE_URL, por defecto http://localhost:8001.
    """
    return os.getenv("RAG_BASE_URL", "http://localhost:8001")


def call_rag_api(
    method: str,
    path: str,
    *,
    params: Optional[Dict[str, Any]] = None,
    json_body: Optional[Dict[str, Any]] = None,
    files: Optional[Dict[str, Any]] = None,
) -> Optional[Dict[str, Any]]:
    """Wrapper simple para llamar al servicio RAG y manejar errores en la UI."""
    url = f"{get_base_url().rstrip('/')}{path}"
    try:
        if method.upper() == "GET":
            resp = requests.get(url, params=params, timeout=60)
        elif method.upper() == "POST":
            resp = requests.post(url, params=params, json=json_body, files=files, timeout=300)
        elif method.upper() == "DELETE":
            resp = requests.delete(url, params=params, timeout=60)
        else:
            st.error(f"Método HTTP no soportado: {method}")
            return None

        if resp.status_code >= 400:
            try:
                detail = resp.json()
            except Exception:
                detail = resp.text
            st.error(f"Error {resp.status_code} llamando a {path}: {detail}")
            return None

        if resp.content:
            try:
                return resp.json()
            except Exception:
                # Si no es JSON, mostrar texto
                st.warning("Respuesta no JSON recibida desde el servicio RAG.")
                st.text(resp.text)
                return None
        return None
    except requests.RequestException as e:
        st.error(f"No se pudo conectar con el servicio RAG en {url}: {e}")
        return None


def ui_header():
    st.set_page_config(
        page_title="RAG Budget Dashboard",
        layout="wide",
    )
    st.title("🧠 RAG Budget Dashboard")
    st.caption(
        "Pequeño front en Streamlit para explorar el servicio RAG: consultas, documentos, presupuestos y métricas."
    )


def sidebar_config():
    st.sidebar.header("⚙️ Configuración")
    base_url_default = get_base_url()
    base_url = st.sidebar.text_input(
        "URL del servicio RAG (FastAPI)",
        value=base_url_default,
        help="Normalmente http://localhost:8001",
    )
    # Guardar en variable de entorno para esta sesión de proceso
    os.environ["RAG_BASE_URL"] = base_url

    st.sidebar.markdown("---")
    st.sidebar.write("### Proyecto por defecto")
    default_project_id = st.sidebar.number_input(
        "ID de proyecto", min_value=1, value=1, step=1
    )
    return default_project_id


def tab_query(project_id_default: int):
    st.subheader("🔎 Consulta RAG sobre documentos")

    col_q1, col_q2 = st.columns([2, 1])
    with col_q1:
        question = st.text_area(
            "Pregunta",
            value="Genera un RESUMEN EJECUTIVO COMPLETO de este proyecto.",
            height=120,
        )
    with col_q2:
        project_id = st.number_input(
            "Project ID (opcional)",
            min_value=0,
            value=project_id_default,
            step=1,
            help="0 para no filtrar por proyecto.",
        )
        top_k = st.slider("Top K (número de chunks)", min_value=50, max_value=100, value=50)

    if st.button("Ejecutar consulta RAG", type="primary"):
        if not question.strip():
            st.warning("Por favor escribe una pregunta.")
            return

        payload: Dict[str, Any] = {
            "question": question.strip(),
            "top_k": top_k,
        }
        if project_id > 0:
            payload["project_id"] = project_id

        with st.spinner("Consultando servicio RAG..."):
            result = call_rag_api("POST", "/query", json_body=payload)

        if not result:
            return

        # Mostrar respuesta principal
        st.markdown("#### 🧾 Respuesta generada")
        st.write(result.get("answer", "Sin respuesta."))

        # Métricas básicas
        st.markdown("#### 📊 Métricas de la consulta")
        col_m1, col_m2 = st.columns(2)
        with col_m1:
            st.metric("Confianza promedio", f"{result.get('confidence', 0.0):.2f}")
        with col_m2:
            n_sources = len(result.get("sources", []))
            st.metric("Número de fuentes", n_sources)

        # Tabla de fuentes + gráfico de similitud
        sources = result.get("sources", [])
        if sources:
            st.markdown("#### 📚 Fuentes utilizadas")

            # Tabla con contenido recortado
            table_rows = []
            for i, s in enumerate(sources, start=1):
                meta = s.get("metadata", {}) or {}
                content = s.get("content", "")
                table_rows.append(
                    {
                        "idx": i,
                        "similarity": s.get("similarity", 0.0),
                        "filename": meta.get("filename", ""),
                        "project_id": meta.get("project_id", ""),
                        "document_type": meta.get("document_type", ""),
                        "preview": (content[:150] + "…") if len(content) > 150 else content,
                    }
                )

            df_sources = pd.DataFrame(table_rows)
            st.dataframe(df_sources, use_container_width=True, hide_index=True)

            # Gráfico de barras de similitud
            st.markdown("##### Distribución de similitud de las fuentes")
            chart_df = df_sources[["idx", "similarity"]].set_index("idx")
            st.bar_chart(chart_df)


def tab_documents(project_id_default: int):
    st.subheader("📄 Gestión de documentos de proyecto")

    col_u1, col_u2 = st.columns([2, 1])
    with col_u1:
        upload_file = st.file_uploader(
            "Subir documento para indexar en RAG",
            type=["pdf", "docx", "txt", "xlsx"],
        )
    with col_u2:
        project_id = st.number_input(
            "Project ID para el documento",
            min_value=0,
            value=project_id_default,
            step=1,
        )
        document_type = st.selectbox(
            "Tipo de documento",
            options=["project_document", "budget", "technical", "other"],
            index=0,
        )

    if st.button("Subir y procesar documento") and upload_file is not None:
        files = {
            "file": (upload_file.name, upload_file.getvalue(), upload_file.type),
        }
        params: Dict[str, Any] = {
            "document_type": document_type,
        }
        if project_id > 0:
            params["project_id"] = project_id

        with st.spinner("Subiendo y procesando documento..."):
            result = call_rag_api("POST", "/documents/upload", params=params, files=files)

        if result:
            st.success("Documento procesado exitosamente.")
            st.json(result)

    st.markdown("---")
    st.markdown("### 📚 Documentos del proyecto")
    list_project_id = st.number_input(
        "Project ID para listar documentos",
        min_value=1,
        value=project_id_default,
        step=1,
        key="docs_project_id",
    )

    if st.button("Listar documentos del proyecto"):
        with st.spinner("Obteniendo documentos desde RAG..."):
            result = call_rag_api("GET", f"/projects/{list_project_id}/documents")

        if not result:
            return

        docs = result.get("documents", []) or result.get("Documents", []) or []
        if not docs:
            st.info("No se encontraron documentos para este proyecto.")
            return

        rows = []
        for d in docs:
            rows.append(
                {
                    "filename": d.get("filename", ""),
                    "document_type": d.get("document_type", ""),
                    "project_id": d.get("project_id", ""),
                    "upload_date": d.get("upload_date", ""),
                    "content_preview": (d.get("content_preview", "")[:150] + "…"),
                }
            )

        df_docs = pd.DataFrame(rows)
        st.dataframe(df_docs, use_container_width=True, hide_index=True)


def tab_budget(project_id_default: int):
    st.subheader("💰 Generación y visualización de presupuestos")

    with st.form("budget_generate_form"):
        col1, col2 = st.columns(2)
        with col1:
            project_id = st.number_input(
                "Project ID",
                min_value=1,
                value=project_id_default,
                step=1,
            )
            duration_years = st.number_input(
                "Duración (años)", min_value=1, value=1, step=1
            )
        with col2:
            project_description = st.text_area(
                "Descripción del proyecto",
                value="Desarrollo de plataforma de telemedicina cardiovascular.",
                height=100,
            )

        categories_default = [
            "TalentoHumano",
            "ServiciosTecnologicos",
            "EquiposSoftware",
            "MaterialesInsumos",
            "CapacitacionEventos",
            "GastosViaje",
        ]
        budget_categories = st.multiselect(
            "Rubros a incluir",
            options=categories_default,
            default=categories_default,
        )

        submitted = st.form_submit_button("Generar presupuesto con RAG/LLM", type="primary")

    if submitted:
        payload: Dict[str, Any] = {
            "project_id": int(project_id),
            "project_description": project_description.strip(),
            "duration_years": int(duration_years),
            "budget_categories": budget_categories,
        }

        with st.spinner("Generando presupuesto (esto puede tardar unos segundos)..."):
            result = call_rag_api("POST", "/budget/generate", json_body=payload)

        if not result:
            return

        st.markdown("#### 📑 Resumen del presupuesto generado")
        method = result.get("method", "desconocido")
        confidence = result.get("confidence_score", 0.0)
        total_budget = result.get("budget_data", {}).get("total_budget", 0.0)
        source_docs_raw = result.get("source_documents", [])
        source_activities_raw = result.get("source_activities", [])
        
        # Convertir a valores simples para st.metric - manejo robusto
        if isinstance(source_docs_raw, list):
            source_docs = len(source_docs_raw)
        elif isinstance(source_docs_raw, int):
            source_docs = source_docs_raw
        else:
            source_docs = 0
            
        if isinstance(source_activities_raw, list):
            source_activities = len(source_activities_raw)
        elif isinstance(source_activities_raw, int):
            source_activities = source_activities_raw
        else:
            source_activities = None

        col_m1, col_m2, col_m3 = st.columns(3)
        with col_m1:
            st.metric("Método", method)
        with col_m2:
            st.metric("Confianza", f"{confidence:.2f}")
        with col_m3:
            st.metric("Total presupuesto (estimado)", f"{total_budget:,.0f}")

        col_s1, col_s2 = st.columns(2)
        with col_s1:
            st.metric("Documentos fuente", source_docs)
        with col_s2:
            if source_activities is not None:
                st.metric("Actividades fuente", source_activities)

        # Mostrar link o ruta al Excel si existe
        excel_path = result.get("excel_path")
        if excel_path:
            st.info(f"Archivo Excel generado: `{excel_path}`")

        # Visualización por categoría
        budget_data = result.get("budget_data", {})
        categories = budget_data.get("categories", {})

        if categories:
            st.markdown("#### 📊 Presupuesto por rubro")
            rows = []
            for cat_name, cat_data in categories.items():
                rows.append(
                    {
                        "rubro": cat_name,
                        "descripcion": cat_data.get("description", ""),
                        "subtotal": cat_data.get("subtotal", 0.0),
                        "confidence": cat_data.get("confidence", 0.0),
                    }
                )
            df_cats = pd.DataFrame(rows)
            st.dataframe(df_cats, use_container_width=True, hide_index=True)

            chart_df = df_cats.set_index("rubro")[["subtotal"]]
            st.bar_chart(chart_df)

            st.markdown("#### 📋 Detalle de ítems por rubro")
            selected_rubro = st.selectbox(
                "Selecciona un rubro para ver el detalle",
                options=list(categories.keys()),
            )
            if selected_rubro:
                items = categories[selected_rubro].get("items", [])
                if items:
                    df_items = pd.DataFrame(items)
                    st.dataframe(df_items, use_container_width=True, hide_index=True)
                else:
                    st.info("Este rubro no tiene ítems detallados.")
        else:
            st.info("No se encontraron categorías en la respuesta de presupuesto.")


def tab_suggestions(project_id_default: int):
    st.subheader("🧩 Sugerencias y análisis de presupuesto por rubro")

    col1, col2 = st.columns(2)
    with col1:
        project_id = st.number_input(
            "Project ID",
            min_value=1,
            value=project_id_default,
            step=1,
            key="suggestions_project_id",
        )
    with col2:
        category = st.selectbox(
            "Filtrar por categoría (opcional)",
            options=[
                "",
                "TalentoHumano",
                "ServiciosTecnologicos",
                "EquiposSoftware",
                "MaterialesInsumos",
                "CapacitacionEventos",
                "GastosViaje",
            ],
        )

    if st.button("Obtener sugerencias de presupuesto"):
        params: Dict[str, Any] = {}
        if category:
            params["category"] = category

        with st.spinner("Consultando sugerencias en el servicio RAG..."):
            result = call_rag_api(
                "GET",
                f"/projects/{int(project_id)}/budget/suggestions",
                params=params,
            )

        if not result:
            return

        suggestions = result.get("suggestions", []) or result.get("Suggestions", [])
        if not suggestions:
            st.info("No se encontraron sugerencias para este proyecto.")
            return

        st.markdown("#### 📌 Sugerencias encontradas")
        rows_summary: List[Dict[str, Any]] = []
        for s in suggestions:
            cat = s.get("category", "")
            items = s.get("suggested_items", [])
            rows_summary.append(
                {
                    "rubro": cat,
                    "num_items": len(items),
                    "confidence": s.get("confidence", 0.0),
                }
            )

        df_summary = pd.DataFrame(rows_summary)
        st.dataframe(df_summary, use_container_width=True, hide_index=True)

        # Gráfico de número de ítems sugeridos por rubro
        st.markdown("##### Número de ítems sugeridos por rubro")
        chart_df = df_summary.set_index("rubro")[["num_items"]]
        st.bar_chart(chart_df)

        # Mostrar detalle por rubro seleccionado
        selected_rubro = st.selectbox(
            "Selecciona un rubro para ver ítems sugeridos",
            options=[row["rubro"] for row in rows_summary],
        )

        if selected_rubro:
            for s in suggestions:
                if s.get("category") == selected_rubro:
                    items = s.get("suggested_items", [])
                    if items:
                        df_items = pd.DataFrame(items)
                        st.dataframe(df_items, use_container_width=True, hide_index=True)
                    st.markdown("**Razonamiento del modelo:**")
                    st.write(s.get("reasoning", ""))
                    break


def tab_cotizacion():
    """Tab para generar cotizaciones desde Excel"""
    st.subheader("📋 Generación de Cotizaciones")
    st.caption(
        "Carga un archivo Excel y genera una cotización formal en formato colombiano. "
        "El sistema valida ítems, agrupa por actividad y genera una tabla markdown profesional."
    )
    
    with st.form("cotizacion_form"):
        col1, col2 = st.columns(2)
        
        with col1:
            upload_file = st.file_uploader(
                "Cargar archivo Excel",
                type=["xlsx", "xls"],
                help="El archivo debe contener columnas: ACTIVIDAD, CANTIDAD, VALOR UNITARIO"
            )
        
        with col2:
            incluir_iva = st.checkbox(
                "Incluir IVA (19%)",
                value=False,
                help="Si está marcado, se incluirá el IVA del 19% en la cotización"
            )
            tasa_iva = st.number_input(
                "Tasa de IVA (%)",
                min_value=0.0,
                max_value=100.0,
                value=19.0,
                step=0.1,
                help="Tasa de IVA a aplicar (por defecto 19%)"
            ) / 100.0  # Convertir a decimal
        
        submitted = st.form_submit_button("Generar Cotización", type="primary")
    
    if submitted and upload_file is not None:
        # Preparar archivo para envío
        files = {
            "file": (upload_file.name, upload_file.getvalue(), upload_file.type)
        }
        
        params = {
            "incluir_iva": incluir_iva,
            "tasa_iva": tasa_iva
        }
        
        with st.spinner("Generando cotización (esto puede tardar unos segundos)..."):
            result = call_rag_api("POST", "/cotizacion/generar", params=params, files=files)
        
        if not result:
            return
        
        # Mostrar cotización
        st.markdown("#### 📄 Cotización Generada")
        
        # Mostrar tabla markdown
        cotizacion_md = result.get("cotizacion_markdown", "")
        if cotizacion_md:
            st.markdown(cotizacion_md)
        
        # Mostrar totales
        st.markdown("---")
        st.markdown("#### 💰 Resumen de Totales")
        
        totales = result.get("totales", {})
        col_t1, col_t2, col_t3 = st.columns(3)
        
        with col_t1:
            st.metric(
                "Subtotal",
                f"${totales.get('subtotal', 0):,.0f} COP"
            )
        
        if incluir_iva:
            with col_t2:
                st.metric(
                    "IVA (19%)",
                    f"${totales.get('iva', 0):,.0f} COP"
                )
            with col_t3:
                st.metric(
                    "Total con IVA",
                    f"${totales.get('total', 0):,.0f} COP"
                )
        else:
            with col_t2:
                st.metric(
                    "Total General",
                    f"${totales.get('subtotal', 0):,.0f} COP"
                )
        
        # Información adicional
        st.markdown("---")
        st.markdown("#### ℹ️ Información de la Cotización")
        
        col_info1, col_info2 = st.columns(2)
        with col_info1:
            st.metric("Total de ítems", result.get("total_items", 0))
            st.metric("Incluye IVA", "Sí" if result.get("incluye_iva", False) else "No")
        with col_info2:
            fecha_gen = result.get("fecha_generacion", "")
            if fecha_gen:
                from datetime import datetime
                try:
                    fecha_obj = datetime.fromisoformat(fecha_gen.replace('Z', '+00:00'))
                    st.metric("Fecha de generación", fecha_obj.strftime("%Y-%m-%d %H:%M:%S"))
                except:
                    st.metric("Fecha de generación", fecha_gen)
        
        # Botones de descarga
        st.markdown("---")
        st.markdown("#### 💾 Descargar Cotización")
        
        col_d1, col_d2 = st.columns(2)
        
        with col_d1:
            # Descargar como Markdown
            st.download_button(
                label="📄 Descargar como Markdown (.md)",
                data=cotizacion_md,
                file_name=f"cotizacion_{datetime.now().strftime('%Y%m%d_%H%M%S')}.md",
                mime="text/markdown"
            )
        
        with col_d2:
            # Convertir a HTML y descargar
            try:
                import markdown
                html_content = markdown.markdown(cotizacion_md, extensions=['tables'])
                html_completo = f"""
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>Cotización</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 40px;
            line-height: 1.6;
        }}
        table {{
            border-collapse: collapse;
            width: 100%;
            margin: 20px 0;
        }}
        th, td {{
            border: 1px solid #ddd;
            padding: 12px;
            text-align: left;
        }}
        th {{
            background-color: #4CAF50;
            color: white;
            font-weight: bold;
        }}
        tr:nth-child(even) {{
            background-color: #f2f2f2;
        }}
        .total-row {{
            font-weight: bold;
            background-color: #e7f3ff;
        }}
    </style>
</head>
<body>
    <h1>Cotización</h1>
    {html_content}
    <p><small>Generada el {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}</small></p>
</body>
</html>
"""
                st.download_button(
                    label="🌐 Descargar como HTML (.html)",
                    data=html_completo,
                    file_name=f"cotizacion_{datetime.now().strftime('%Y%m%d_%H%M%S')}.html",
                    mime="text/html"
                )
            except ImportError:
                st.info("Para descargar HTML, instala: pip install markdown")
            except Exception as e:
                st.warning(f"No se pudo generar HTML: {str(e)}")
        
        # Mostrar ítems en tabla (opcional, para revisión)
        with st.expander("📊 Ver ítems detallados"):
            items = result.get("items", [])
            if items:
                df_items = pd.DataFrame(items)
                st.dataframe(df_items, use_container_width=True, hide_index=True)
    
    elif submitted and upload_file is None:
        st.warning("Por favor, carga un archivo Excel para generar la cotización.")
    
    # Instrucciones
    with st.expander("ℹ️ Instrucciones para el formato del Excel"):
        st.markdown("""
        ### Formato Requerido del Archivo Excel
        
        El archivo Excel debe contener las siguientes columnas (el sistema las detecta automáticamente):
        
        **Columnas Obligatorias:**
        - **ACTIVIDAD**: Nombre o descripción del ítem a cotizar
        - **CANTIDAD**: Cantidad numérica mayor a 0
        - **VALOR UNITARIO**: Valor unitario en pesos colombianos (mayor a 0)
        
        **Columnas Opcionales:**
        - **VALOR TOTAL**: Si no está presente, se calcula como CANTIDAD × VALOR UNITARIO
        - **JUSTIFICACIÓN**: Observaciones o justificación del ítem
        
        ### Ejemplo de Formato:
        
        | ACTIVIDAD | CANTIDAD | VALOR UNITARIO | VALOR TOTAL | JUSTIFICACIÓN |
        |-----------|----------|----------------|-------------|---------------|
        | Servicio de consultoría técnica | 40 | 150000 | 6000000 | Horas de consultoría especializada |
        | Licencias de software | 10 | 500000 | 5000000 | Licencias anuales |
        
        ### Validaciones:
        - Se ignoran filas con palabras: TOTAL, SUBTOTAL, NOTA, etc.
        - CANTIDAD debe ser numérica y mayor a 0
        - VALOR UNITARIO debe ser numérico y mayor a 0
        - Los ítems se agrupan automáticamente por actividad
        """)


def main():
    ui_header()
    default_project_id = sidebar_config()

    tab_labels = [
        "Consulta RAG",
        "Documentos",
        "Presupuesto",
        "Cotizaciones",
        "Sugerencias / Análisis",
    ]
    tab_query_ui, tab_docs_ui, tab_budget_ui, tab_cotizacion_ui, tab_sugg_ui = st.tabs(tab_labels)

    with tab_query_ui:
        tab_query(default_project_id)
    with tab_docs_ui:
        tab_documents(default_project_id)
    with tab_budget_ui:
        tab_budget(default_project_id)
    with tab_cotizacion_ui:
        tab_cotizacion()
    with tab_sugg_ui:
        tab_suggestions(default_project_id)


if __name__ == "__main__":
    main()


