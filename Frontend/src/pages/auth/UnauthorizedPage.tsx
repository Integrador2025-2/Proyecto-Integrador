import { useNavigate } from 'react-router-dom'
import { Button } from '../../components/ui/button'
import {
    Card,
    CardContent,
    CardDescription,
    CardFooter,
    CardHeader,
    CardTitle,
} from '../../components/ui/card'

export const UnauthorizedPage = () => {
    const navigate = useNavigate()

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-50">
            <Card className="w-full max-w-md">
                <CardHeader className="space-y-1">
                    <CardTitle className="text-2xl font-bold text-center text-red-600">
                        Acceso Denegado
                    </CardTitle>
                    <CardDescription className="text-center">
                        No tiene permisos para acceder a esta página
                    </CardDescription>
                </CardHeader>

                <CardContent className="flex flex-col items-center space-y-4">
                    <div className="rounded-full bg-red-100 p-6">
                        <svg
                            className="h-16 w-16 text-red-600"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z"
                            />
                        </svg>
                    </div>

                    <p className="text-center text-gray-600">
                        Si cree que esto es un error, contacte al administrador del sistema.
                    </p>
                </CardContent>

                <CardFooter className="flex justify-center space-x-4">
                    <Button variant="outline" onClick={() => navigate(-1)}>
                        Volver
                    </Button>
                    <Button onClick={() => navigate('/')}>Ir al Inicio</Button>
                </CardFooter>
            </Card>
        </div>
    )
}
