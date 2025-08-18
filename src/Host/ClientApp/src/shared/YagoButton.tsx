import { Button } from "@mui/material";
import React from "react";

interface ButtonOnClickProps {
    onClick: (() => void) | undefined;
    text: string;
    isDisabled: boolean;
}

const YagoButton: React.FC<ButtonOnClickProps> = ({ onClick, text, isDisabled = false }) => {
    return (
        <Button
            onClick={onClick}
            variant="outlined"
            sx={{
                margin: { xs: '4px', sm: '0.5rem' },
                padding: { xs: '4px 10px', sm: '5px 15px' },
                textDecoration: 'none',
                color: 'inherit'
            }}
            disabled={isDisabled} >
            {text}
        </ Button >
    )
}

export default YagoButton
