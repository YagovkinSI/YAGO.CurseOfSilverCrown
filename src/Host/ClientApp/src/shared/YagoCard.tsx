interface YagoCardProps {
    children?: React.ReactNode;
    className?: string;
    variant?: 'default' | 'glow' | 'error' | 'success';
}

const YagoCard: React.FC<YagoCardProps> = ({
    children,
    className = '',
    variant = 'default'
}) => {
    const variantClasses = {
        default: 'border-bright/10 shadow-[0_0_60px_rgba(240,230,92,0.05)]',
        glow: 'border-bright/20 shadow-[0_0_80px_rgba(240,230,92,0.1)]',
        error: 'border-danger/30 shadow-[0_0_60px_rgba(211,47,47,0.1)]',
        success: 'border-good/30 shadow-[0_0_60px_rgba(76,175,80,0.1)]',
    };

    return (
        <div className={`
            flex flex-col items-center gap-6 md:gap-8 mx-auto px-4
            bg-dark/60 backdrop-blur-sm border rounded-2xl p-8 md:p-12 w-full max-w-md
            ${variantClasses[variant]}
            ${className}
        `}>
            {children}
        </div>
    );
};

export default YagoCard;