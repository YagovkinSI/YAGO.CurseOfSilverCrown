import React from 'react';
import { Clock } from 'lucide-react';
import InputText from '../../shared/ui/InputText';
import SlideButtons from './SlideButtons';
import type { SlideActions, SlideInputState } from '../SlideRenderer';
import type { SlideButton } from '../../entities/events/colonyEvent.types';
import Surface from '../../shared/ui/Surface';

interface SlideBottomPanelProps {
    buttons: SlideButton[];
    actions?: SlideActions;
    inputState?: SlideInputState;
    renderBottomSlot?: React.ReactNode;
    createdAt?: string;
}

const SlideBottomPanel: React.FC<SlideBottomPanelProps> = ({
    buttons,
    actions,
    inputState,
    renderBottomSlot,
    createdAt,
}) => {
    const hasTextInput = buttons.some(b => b.action?.type == 'inputCompleted');

    const renderInput = () => {
        if (!hasTextInput) return null;
        return (
            <InputText
                name="slideInputText"
                label="Название колонии"
                type="text"
                value={inputState?.value ?? ''}
                handleChange={inputState?.onChange ?? (() => { })}
                handleBlur={inputState?.onChange ?? (() => { })}
                error={!!inputState?.error}
                helperText={inputState?.error}
            />
        );
    };

    const renderCreatedAt = () => {
        if (!createdAt) return null;
        return (
            <div className="flex items-center gap-2 pt-1">
                <Clock className="w-3 h-3 text-muted/30" />
                <span className="text-[0.55rem] text-muted/30">
                    {createdAt}
                </span>
            </div>
        );
    };

    return (
        <Surface rounded='md' variant='default' className='p-3 mt-2 flex flex-col gap-2 overflow-y-auto'>
            <div className="flex-shrink-0 py-2 space-y-2 mx-auto w-full">
                {renderInput()}
                <SlideButtons
                    buttons={buttons}
                    inputTextValue={inputState?.value}
                    inputTextError={inputState?.error}
                    onButtonClick={actions?.onButtonClick}
                    onInfoSlideClick={actions?.onInfoSlideClick}
                    onSlideChange={actions?.onSlideChange}
                />
                {renderBottomSlot}
                {renderCreatedAt()}
            </div>
        </Surface>
    );
};

export default SlideBottomPanel;