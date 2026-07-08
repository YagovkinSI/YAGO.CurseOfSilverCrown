import React from 'react';

interface TextFooterCommentProps {
    children?: React.ReactNode;
}

const TextFooterComment: React.FC<TextFooterCommentProps> = ({ children }) => {
    const getTextStyles = () => {
        return 'text-muted text-xs font-light tracking-wide my-2';
    };

    return (
        <p className={getTextStyles()}>
            {children}
        </p>
    );
};

export default TextFooterComment;