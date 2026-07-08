import { Box, useMediaQuery, useTheme, type SxProps, type Theme } from '@mui/material';
import React from 'react';
import type { RowDataProps } from './RowData';
import RowData from './RowData';

interface StateListProps {
    items: RowDataProps[],
    sx?: SxProps<Theme>
}

const StateList: React.FC<StateListProps> = ({ items, sx }) => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    return (
        <Box
            display="flex"
            flexDirection="column"
            gap={1}
            sx={{
                width: '100%',
                maxWidth: isMobile ? 350 : 700,
                margin: '0 auto',
                ...sx,
            }}
        >
            {items.map((rowData, index) => (
                <React.Fragment key={index}>
                    <RowData key={rowData.label} {...rowData}></RowData>
                </React.Fragment>
            ))}
        </Box>
    )
}

export default StateList