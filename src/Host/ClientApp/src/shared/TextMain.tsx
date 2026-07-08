import React from 'react';

interface TextMainProps {
    textArray: string[];
    className?: string;
}

const TextMain: React.FC<TextMainProps> = ({ textArray, className = '' }) => {
    return (
        <>
            {textArray.map((t, index) => (
                <p 
                    key={index}
                    className={`text-justify mb-4 ${className}`}
                >
                    {t}
                </p>
            ))}
        </>
    );
};

export default TextMain;