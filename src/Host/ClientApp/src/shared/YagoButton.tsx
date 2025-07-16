import { Button } from "@mui/material";
import React from "react";

interface ButtonOnClickProps {
    onClick: (() => void) | undefined;
    text: string;
    isDisabled: boolean;
}

const YagoButton: React.FC<ButtonOnClickProps> = ({ onClick, text, isDisabled = false }) => {
    return (
        <Button onClick={onClick} variant="outlined" style={{ margin: '0.5rem', textDecoration: 'none', color: 'inherit' }} disabled={isDisabled} >
            {text}
        </ Button >
    )
}

export default YagoButton
