import { Button, type ButtonPropsColorOverrides, type ButtonPropsVariantOverrides } from "@mui/material";
import type { OverridableStringUnion } from "@mui/types";
import React from "react";

interface ButtonOnClickProps {
    onClick: (() => void) | undefined;
    text: string;
    variant?: OverridableStringUnion<'text' | 'outlined' | 'contained', ButtonPropsVariantOverrides>;
    color?: OverridableStringUnion<'inherit' | 'primary' | 'secondary' | 'success' | 'error' | 'info' | 'warning', ButtonPropsColorOverrides>;
    isDisabled?: boolean;
}

const YagoButton: React.FC<ButtonOnClickProps> = ({ onClick, text, variant = "outlined", color = 'primary', isDisabled = false }) => {
    return (
        <Button
            onClick={onClick}
            variant={variant}
            color = {color}
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
