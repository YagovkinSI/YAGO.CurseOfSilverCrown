import React from 'react';
import { AlertCircle } from 'lucide-react';

interface YagoCardContentInputFieldProps {
    value: string;
    label: string;
    handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    error: string;
}

const YagoCardContentInputField: React.FC<YagoCardContentInputFieldProps> = ({ 
    value, 
    label, 
    handleChange, 
    error 
}) => {
    const hasError = !!error;

    const renderLabel = () => (
        <label 
            htmlFor="card-input-field"
            className="block text-sm font-medium text-light/80 mb-1.5"
        >
            {label}
        </label>
    );

    const renderInput = () => (
        <input
            id="card-input-field"
            type="text"
            value={value}
            onChange={handleChange}
            className={`
                w-full px-3 py-2.5 bg-dark/30 border rounded-md 
                text-light placeholder-muted/40
                transition-colors duration-200
                focus:outline-none focus:ring-2 focus:ring-bright/50
                ${hasError 
                    ? 'border-danger focus:border-danger focus:ring-danger/30' 
                    : 'border-muted/30 focus:border-bright/50'
                }
            `}
            aria-invalid={hasError}
            aria-describedby={hasError ? "input-error" : undefined}
        />
    );

    const renderErrorIcon = () => {
        if (!hasError) return null;
        return (
            <div className="absolute right-3 top-1/2 -translate-y-1/2">
                <AlertCircle className="w-5 h-5 text-danger" />
            </div>
        );
    };

    const renderHelperText = () => {
        if (!hasError) return null;
        return (
            <p id="input-error" className="mt-1.5 text-sm text-danger">
                {error}
            </p>
        );
    };

    return (
        <div className="mx-4 text-center">
            <div className="mb-4">
                {renderLabel()}
                <div className="relative">
                    {renderInput()}
                    {renderErrorIcon()}
                </div>
                {renderHelperText()}
            </div>
        </div>
    );
};

export default YagoCardContentInputField;