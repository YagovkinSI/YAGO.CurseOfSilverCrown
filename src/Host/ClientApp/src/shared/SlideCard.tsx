import React from 'react';
import ButtonBack from './ButtonBack';
import Card from './Card';
import Title from './Title';

interface SlideCardProps {
    children?: React.ReactNode;
    title: string;
    image?: string;
    showBackButton?: boolean;
}

const SlideCard: React.FC<SlideCardProps> = ({ 
    children, 
    title, 
    image,
    showBackButton = true,
}) => {
    const renderImage = () => {
        if (!image) return null;
        return (
            <div className="relative w-full pt-[56.25%]">
                <img
                    src={image}
                    alt={title}
                    className="absolute top-0 left-0 w-full h-full object-cover"
                    loading="lazy"
                />
            </div>
        );
    };

    return (
        <Card variant="default" className="relative flex flex-col w-full max-w-2xl mx-auto max-h-[90vh] overflow-y-auto">
            {/* Заголовок с кнопкой Назад */}
            <div className="flex items-center px-4 py-3 border-b border-bright/10 flex-shrink-0">
                <div className="flex-shrink-0">
                    {showBackButton && <ButtonBack />}
                </div>
                <Title className="flex-1 text-center truncate px-2">
                    {title}
                </Title>
                {/* Пустой блок для баланса (такой же ширины как кнопка) */}
                {showBackButton && <div className="w-9 flex-shrink-0" />}
            </div>

            {/* Иллюстрация */}
            {renderImage()}

            {/* Контент */}
            <div className="flex-1 p-4 space-y-4">
                {children}
            </div>
        </Card>
    );
};

export default SlideCard;