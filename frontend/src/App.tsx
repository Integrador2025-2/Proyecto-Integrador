import { useState } from 'react'
import './index.css'
import './App.css'
import { Button } from '@radix-ui/themes'

function App() {
    return (
        <>
            <h1 className="text-3xl font-bold underline text-green-500">Hello world!</h1>
            <Button>Radix Button</Button>
        </>
    )
}

export default App
