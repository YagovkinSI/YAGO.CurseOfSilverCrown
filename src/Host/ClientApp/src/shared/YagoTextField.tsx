import React from 'react';
import { AlertCircle } from 'lucide-react';

interface YagoTextFieldProps {
    name: string;
    label: string;
    value: string;
    autoFocus?: boolean;
    autoComplete: string;
    error?: boolean;
    helperText?: React.ReactNode;
    handleChange: (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => void;
    handleBlur: (e: React.FocusEvent<HTMLInputElement | HTMLTextAreaElement, Element>) => void;
    type?: string;
}

const YagoTextField: React.FC<YagoTextFieldProps> = (props) => {
    const {
        name,
        label,
        value,
        autoFocus,
        autoComplete,
        error,
        helperText,
        handleChange,
        handleBlur,
        type = 'text',
    } = props;

    const renderLabel = () => (
        <label
            htmlFor={name}
            className="block text-sm font-medium text-light mb-1.5"
        >
            {label}
            <span className="text-danger ml-0.5">*</span>
        </label>
    );

    const renderInput = () => {
        const baseClasses = `
            w-full px-3 py-2.5 bg-dark/50 border rounded-md text-light placeholder-muted/60 
            transition-colors duration-200 focus:outline-none focus:ring-2`;
        const statusClasses = error
            ? 'border-danger focus:border-danger focus:ring-danger/30'
            : 'border-muted/30 focus:border-bright/50 focus:ring-bright/50';
        return (
            <input
                id={name}
                name={name}
                type={type}
                value={value}
                autoFocus={autoFocus}
                autoComplete={autoComplete}
                onChange={handleChange}
                onBlur={handleBlur}
                className={`${baseClasses} ${statusClasses}`}
                required
            />
        );
    };

    const renderErrorState = () => {
        if (!error) return null;
        return (
            <>
                <div className="absolute right-3 top-1/2 -translate-y-1/2">
                    <AlertCircle className="w-5 h-5 text-danger" />
                </div>
                {helperText && (
                    <p className="mt-1.5 text-sm text-danger">{helperText}</p>
                )}
            </>
        );
    };

    return (
        <div className="mb-4 w-full">
            {renderLabel()}
            <div className="relative">
                {renderInput()}
                {renderErrorState()}
            </div>
        </div>
    );
};

export default YagoTextField;