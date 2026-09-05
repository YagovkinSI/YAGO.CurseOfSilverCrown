import React from 'react';
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
}

const SlideBottomPanel: React.FC<SlideBottomPanelProps> = ({
    buttons,
    actions,
    inputState,
    renderBottomSlot,
}) => {
    const hasTextInput = buttons.some(b => b.action?.needsInput);

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

    return (
        <Surface rounded='md' variant='default' className='p-3 mt-2 flex flex-col gap-2 overflow-y-auto'>
            <div className="flex-shrink-0 space-y-2 mx-auto w-full">
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
            </div>
        </Surface>
    );
};

export default SlideBottomPanel;