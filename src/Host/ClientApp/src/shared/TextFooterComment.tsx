import React from 'react';
import { Typography } from '@mui/material';

interface TextFooterCommentProps {
    children?: React.ReactNode;
}

const TextFooterComment: React.FC<TextFooterCommentProps> = ({ children }) => {

    return (
        <Typography 
            className='text-footer' 
            sx={{
                fontWeight: 300,
                margin: '8px 0',
                letterSpacing: '0.5px',
                fontSize: '0.8rem'
            }}>
            {children}
        </Typography>
    )
}

export default TextFooterComment;

