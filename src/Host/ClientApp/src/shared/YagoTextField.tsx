import { TextField } from "@mui/material";
import React from "react";

interface YagoTextFieldProps {
    name: string,
    label: string,
    value: string,
    autoFocus?: boolean | undefined,
    autoComplete: string,
    error: boolean | undefined,
    helperText: React.ReactNode,
    handleChange: (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => void,
    handleBlur: (e: React.FocusEvent<HTMLInputElement | HTMLTextAreaElement, Element>) => void,
    type?: string | undefined,
}

const YagoTextField: React.FC<YagoTextFieldProps> = (props) => {
    return (
        <TextField
            margin="normal"
            required
            fullWidth
            id={props.name}
            label={props.label}
            name={props.name}
            type={props.type}
            autoComplete={props.autoComplete}
            autoFocus={props.autoFocus}
            value={props.value}
            onChange={props.handleChange}
            onBlur={props.handleBlur}
            error={props.error}
            helperText={props.helperText}

        />
    )
}

export default YagoTextField
