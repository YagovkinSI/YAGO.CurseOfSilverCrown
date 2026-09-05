import React from 'react';
import { useNavigate } from 'react-router-dom';
import { HelpCircle } from 'lucide-react';
import Button from '../../shared/ui/buttons/Button';
import type { SlideButton } from '../../entities/events/colonyEvent.types';

interface SlideButtonsProps {
    buttons: SlideButton[];
    inputTextValue?: string;
    inputTextError?: string;
    onButtonClick?: (button: SlideButton) => void;
    onInfoSlideClick?: (slideId: string) => void;
    onSlideChange?: (slideId: string) => void;
}

const SlideButtons: React.FC<SlideButtonsProps> = ({
    buttons,
    inputTextValue,
    inputTextError,
    onButtonClick,
    onInfoSlideClick,
    onSlideChange,
}) => {
    const navigate = useNavigate();

    const handleButtonClick = (button: SlideButton) => {
        if (button.action && onButtonClick) {
            onButtonClick(button);
        } else if (button.navigate) {
            navigate(button.navigate.actionUrl);
        } else if (button.toSlide && onSlideChange) {
            onSlideChange(button.toSlide.slideId);
        }
    };

    const isDisabled = (button: SlideButton) => {
        const needInput = button.action?.needsInput;
        return !button.isAvailable
            || (needInput && (!!inputTextError || (inputTextValue?.length ?? 0) < 2));
    };

    const renderInfoButton = (button: SlideButton) => {
        if (!button.infoSlideId || !onInfoSlideClick) return null;
        return (
            <button
                onClick={() => onInfoSlideClick(button.infoSlideId!)}
                className="flex-shrink-0 w-10 h-10 rounded-lg border border-bright/20
                    text-muted hover:text-light hover:border-bright/40 transition-colors flex items-center justify-center"
                aria-label="Подробнее"
            >
                <HelpCircle className="w-4 h-4" />
            </button>
        );
    };

    const renderButton = (button: SlideButton, index: number) => (
        <div key={index} className="flex items-center gap-2">
            <Button
                variant={button.action != undefined ? 'primary' : 'secondary'}
                sizeSm="sm"
                sizeMd="md"
                onClick={() => handleButtonClick(button)}
                disabled={isDisabled(button)}
                className="flex-1"
            >
                {button.name}
            </Button>
            {renderInfoButton(button)}
        </div>
    );

    return (
        <div className="flex flex-col gap-2 w-full">
            {buttons.map((button, index) => renderButton(button, index))}
        </div>
    );
};

export default SlideButtons;