interface NavButtonProps {
    icon: React.ReactNode;
    label: string;
    onClick: () => void;
    badge?: number;
    isActive?: boolean;
}

const NavButton: React.FC<NavButtonProps> = ({
    icon,
    label,
    onClick,
    badge,
    isActive = true
}) => (
    <button
        onClick={onClick}
        disabled={!isActive}
        className={`
            flex items-center gap-3 px-4 py-3 rounded-lg
            transition-all duration-200
            ${isActive
                ? 'bg-dark/60 backdrop-blur-sm border border-bright/20 text-light hover:bg-dark/80 hover:text-bright hover:border-bright/40 animate-border-pulse'
                : 'cursor-not-allowed text-muted bg-dark/30 backdrop-blur-sm border border-muted/20'
            }
            md:px-5 md:py-3.5
            w-full
        `}
    >
        <span className="relative">
            {icon}
            {badge && (
                <span className="absolute -top-1 -right-2 w-2.5 h-2.5 bg-danger rounded-full animate-pulse" />
            )}
        </span>
        <span className="hidden md:text-base md:inline text-sm font-medium tracking-wide uppercase">
            {label}
        </span>
    </button>
);

export default NavButton;