import React from 'react';
import { useNavigate } from 'react-router-dom';
import { ArrowLeft, BookOpen, HelpCircle } from 'lucide-react';
import Button from '../../shared/ui/buttons/Button';
import { COUNCIL_AVATARS } from '../../entities/council/council.avatars';
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

    const renderRefButton = (slideId: string | undefined, children: React.ReactNode, ariaLabel: string, title: string) => {
        if (!slideId || !onInfoSlideClick) return null;
        return (
            <button
                onClick={() => onInfoSlideClick(slideId)}
                className="shrink-0 self-stretch w-11 md:w-[54px] min-h-11 rounded-lg border border-bright/30 bg-bright/10
                    text-muted hover:text-light hover:border-bright/60 hover:bg-bright/15 active:scale-95
                    transition-all duration-200 cursor-pointer flex items-center justify-center overflow-hidden p-[2px]"
                aria-label={ariaLabel}
                title={title}
            >
                {children}
            </button>
        );
    };

    const renderButton = (button: SlideButton, index: number) => (
        <div key={index} className="flex items-stretch gap-2">
            <Button
                variant={button.action != undefined ? 'primary' : 'secondary'}
                sizeSm="sm"
                sizeMd="md"
                icon={button.kind === 'Return' ? ArrowLeft : button.kind === 'Reference' ? BookOpen : undefined}
                iconPosition="left"
                onClick={() => handleButtonClick(button)}
                disabled={isDisabled(button)}
                className="flex-1"
            >
                {button.name}
            </Button>
            {renderRefButton(button.infoSlideId, <HelpCircle className="w-4 h-4" />, 'Подробнее', 'Подробнее')}
            {renderRefButton(
                button.administratorSlideId,
                <img src={COUNCIL_AVATARS.administrator} alt="" className="w-full h-full object-cover rounded-md" />,
                'Мнение правителя: администратор',
                'Мнение правителя: администратор'
            )}
            {renderRefButton(
                button.engineerSlideId,
                <img src={COUNCIL_AVATARS.engineer} alt="" className="w-full h-full object-cover rounded-md" />,
                'Мнение правителя: инженер станции',
                'Мнение правителя: инженер станции'
            )}
            {renderRefButton(
                button.financierSlideId,
                <img src={COUNCIL_AVATARS.financier} alt="" className="w-full h-full object-cover rounded-md" />,
                'Мнение правителя: финансист',
                'Мнение правителя: финансист'
            )}
            {renderRefButton(
                button.socialSlideId,
                <img src={COUNCIL_AVATARS.social} alt="" className="w-full h-full object-cover rounded-md" />,
                'Мнение правителя: социальный советник',
                'Мнение правителя: социальный советник'
            )}
        </div>
    );

    return (
        <div className="flex flex-col gap-2 w-full">
            {buttons.map((button, index) => renderButton(button, index))}
        </div>
    );
};

export default SlideButtons;
