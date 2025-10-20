import { Button, type ButtonPropsVariantOverrides } from "@mui/material";
import type { OverridableStringUnion } from "@mui/types";
import React from "react";

interface ButtonOnClickProps {
    onClick: (() => void) | undefined;
    text: string;
    variant?: OverridableStringUnion<'text' | 'outlined' | 'contained', ButtonPropsVariantOverrides>;
    isDisabled?: boolean;
}

const YagoButton: React.FC<ButtonOnClickProps> = ({ onClick, text, variant = "outlined", isDisabled = false }) => {
    return (
        <Button
            onClick={onClick}
            variant={variant}
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
