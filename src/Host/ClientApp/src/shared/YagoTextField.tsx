// shared/YagoTextField.tsx
interface YagoTextFieldProps {
    label: string;
    name: string;
    value: string;
    handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    handleBlur: (e: React.FocusEvent<HTMLInputElement>) => void;
    error?: boolean;
    helperText?: string | false;
    type?: string;
    autoComplete?: string;
    autoFocus?: boolean;
    icon?: React.ReactNode;
    placeholder?: string;
}

const YagoTextField: React.FC<YagoTextFieldProps> = ({
    label,
    name,
    value,
    handleChange,
    handleBlur,
    error = false,
    helperText,
    type = 'text',
    autoComplete,
    autoFocus = false,
    icon,
    placeholder,
}) => (
    <div className="relative">
        {icon && (
            <div className="absolute left-3 top-1/2 -translate-y-1/2 text-muted">
                {icon}
            </div>
        )}
        <input
            type={type}
            name={name}
            value={value}
            onChange={handleChange}
            onBlur={handleBlur}
            autoComplete={autoComplete}
            autoFocus={autoFocus}
            placeholder={placeholder || label}
            className={`
                w-full px-4 py-3 bg-dark/50 border rounded-lg text-light placeholder-muted 
                focus:outline-none transition-colors
                ${icon ? 'pl-10' : ''}
                ${error 
                    ? 'border-danger/50 focus:border-danger' 
                    : 'border-bright/20 focus:border-bright/50'
                }
            `}
        />
        {helperText && (
            <p className={`text-xs mt-1 ${error ? 'text-danger' : 'text-muted'}`}>
                {helperText}
            </p>
        )}
    </div>
);

export default YagoTextField;