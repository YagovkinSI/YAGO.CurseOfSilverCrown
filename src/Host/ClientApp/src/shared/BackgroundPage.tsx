import { FlexContainer } from "./FlexContainer";

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
        <FlexContainer
            className={`relative bg-cover bg-center bg-fixed ${className}`}
            style={{ backgroundImage: `url('/images/pictures/${backgroundImage}.jpg')` }}
        >
            {darkenBackground && <div className="absolute inset-0 bg-dark/60 backdrop-blur-[2px]" />}
            {children}
        </FlexContainer>
    );
};