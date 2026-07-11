import React, { type CSSProperties } from 'react';

interface FlexContainerProps {
    children: React.ReactNode;
    items?: 'start' | 'center' | 'end' | 'stretch';
    justify?: 'start' | 'center' | 'end' | 'between' | 'around' | 'evenly' | 'stretch';
    direction?: 'row' | 'col';
    className?: string;
    fullHeight?: boolean;
    fullWidth?: boolean;
    style?: CSSProperties;
}

const itemsMap = {
    start: 'items-start',
    center: 'items-center',
    end: 'items-end',
    stretch: 'items-stretch',
};

const justifyMap = {
    start: 'justify-start',
    center: 'justify-center',
    end: 'justify-end',
    between: 'justify-between',
    around: 'justify-around',
    evenly: 'justify-evenly',
    stretch: 'justify-stretch',
};

const directionMap = {
    row: 'flex-row',
    col: 'flex-col',
};

export const FlexContainer: React.FC<FlexContainerProps> = ({
    children,
    items = 'center',
    justify = 'center',
    direction = 'col',
    className = '',
    fullHeight = true,
    fullWidth = true,
    style,
}) => {
    const heightClass = fullHeight ? 'flex-1 min-h-full' : '';
    const widthClass = fullWidth ? 'w-full' : '';

    return (
        <div 
            className={`flex ${directionMap[direction]} ${itemsMap[items]} ${justifyMap[justify]}
                ${heightClass} ${widthClass} ${className}
            `}
            style={style}
        >
            {children}
        </div>
    );
};