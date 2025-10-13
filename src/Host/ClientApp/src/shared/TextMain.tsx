import React from 'react';
import { Typography, type SxProps, type Theme } from '@mui/material';

interface TextMainProps {
    textArray: string[],
    sx?: SxProps<Theme>
}

const TextMain: React.FC<TextMainProps> = ({ textArray, sx }) => {
    console.log(textArray)
    return (
        <>
            {textArray.map(t =>
                <Typography textAlign="justify" gutterBottom sx={{ ...sx }}>
                    {t}
                </Typography>
            )}
        </>
    )
}

export default TextMain;

