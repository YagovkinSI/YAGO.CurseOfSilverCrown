interface PageBackgroundProps {
    children: React.ReactNode;
    backgroundImage?: string;
    darkenBackground?: boolean;
    className?: string;
}

export const BackgroundPage: React.FC<PageBackgroundProps> = ({
    children,
    backgroundImage,
    darkenBackground = false,
    className = '',
}) => {
    if (!backgroundImage) {
        return children;
    }

    return (
        <div
            className={`h-full w-full relative bg-cover bg-center ${className}`}
            style={{ backgroundImage: `url('/images/pictures/${backgroundImage}.jpg')` }}
        >
            {darkenBackground && <div className="absolute inset-0 bg-dark/60 backdrop-blur-[2px]" />}
            {children}
        </div>
    );
};