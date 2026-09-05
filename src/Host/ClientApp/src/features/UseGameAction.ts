import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUseActionMutation } from '../entities/gameActions/gameActions.api';
import type { SlideButton } from '../entities/events/colonyEvent.types';

const useGameAction = () => {
    const navigate = useNavigate();
    const [performAction, { data, error, isLoading }] = useUseActionMutation();
    const [actionError, setActionError] = useState<string | undefined>(undefined);

    const apply = async (button: SlideButton, inputValue?: string) => {
        if (!button.action) return;
        setActionError(undefined);
        const value = button.action.needsInput
            ? inputValue ?? ''
            : button.action.value;
        try {
            const result = await performAction({
                type: button.action.gameActionType,
                code: button.action.code,
                value,
            }).unwrap();
            if (result.data == undefined || !result.data.show) {
                navigate('/me/colony');
            }
        } catch (e) {
            if (e && typeof e === 'object' && 'data' in e) {
                const errorData = (e as { data?: { title?: string } }).data;
                setActionError(errorData?.title ?? 'Неизвестная ошибка.');
            } else {
                setActionError('Неизвестная ошибка.');
            }
        }
    };

    return {
        apply,
        data,
        isLoading,
        error: actionError ?? error,
    };
};

export default useGameAction;