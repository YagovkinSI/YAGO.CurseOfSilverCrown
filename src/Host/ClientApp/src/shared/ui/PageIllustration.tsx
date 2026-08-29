import React from 'react';

interface PageIllustrationProps {
    image: string;
    title: string;
    subtitle: string;
}

const PageIllustration: React.FC<PageIllustrationProps> = ({ image, title, subtitle }) => (
    <div className="relative rounded-xl overflow-hidden h-32 md:h-48 mb-4">
        <img src={image} className="w-full h-full object-cover" alt={title} />
        <div className="absolute inset-0 bg-gradient-to-t from-dark via-dark/50 to-transparent" />
        <div className="absolute bottom-4 left-4">
            <h2 className="text-lg font-bold text-light">{title}</h2>
            <p className="text-sm text-muted">{subtitle}</p>
        </div>
    </div>
);

export default PageIllustration;
