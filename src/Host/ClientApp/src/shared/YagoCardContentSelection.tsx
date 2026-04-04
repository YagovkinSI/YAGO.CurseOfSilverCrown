import React from 'react';
import { Box, Typography } from '@mui/material';
import { ArrowBack, ArrowForward } from '@mui/icons-material';
import './yagoCardContentSelection.css';

interface YagoCardContentSelectionProps {
    handlePrev: () => void;
    label: string;
    handleNext: () => void;
    disabledPrev?: boolean;
    disabledNext?: boolean;
}

const YagoCardContentSelection: React.FC<YagoCardContentSelectionProps> = ({ 
    handlePrev, 
    label, 
    handleNext,
    disabledPrev = false,
    disabledNext = false 
}) => {
    return (
        <Box className="yago-selection-container">
            {/* Левая линия (декоративная) */}
            <div className="yago-selection__left-line" />
            
            {/* Правая линия (декоративная) */}
            <div className="yago-selection__right-line" />
            
            {/* Верхняя линия */}
            <div className="yago-selection__line yago-selection__line--top" />
            
            {/* Нижняя линия */}
            <div className="yago-selection__line yago-selection__line--bottom" />
            
            <div className="yago-selection__content">
                <button 
                    className="yago-selection__nav-btn yago-selection__nav-btn--prev"
                    onClick={handlePrev}
                    disabled={disabledPrev}
                >
                    <ArrowBack />
                    <span className="yago-selection__nav-tooltip">Назад</span>
                </button>
                
                <div className="yago-selection__label-wrapper">
                    <Typography variant="h6" className="yago-selection__label">
                        {label}
                    </Typography>
                </div>
                
                <button 
                    className="yago-selection__nav-btn yago-selection__nav-btn--next"
                    onClick={handleNext}
                    disabled={disabledNext}
                >
                    <ArrowForward />
                    <span className="yago-selection__nav-tooltip">Вперёд</span>
                </button>
            </div>
        </Box>
    );
};

export default YagoCardContentSelection;