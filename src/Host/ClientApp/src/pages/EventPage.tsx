import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useGetUserPrivateQuery } from "../entities/users/user.api";
import { useCompleteEventMutation, useGetColonyEventQuery } from "../entities/events/colonyEvent.api";
import { SanitizeColonyName, ValidateColonyName } from '../features/ColonyNameValidator';
import { formatTimeAgo } from '../features/TimeHelper';
import Page from '../widgets/Page';
import SlideRenderer from '../widgets/SlideRenderer';
import { ArrowLeft } from 'lucide-react';
import ResultSlideRenderer from '../entities/events/ResultSlideRenderer';
import type { SlideButton, SlideButtonAction } from '../entities/events/colonyEvent.types';

const EventPage: React.FC = () => {
    const { id } = useParams();
    const idAsNumber = id ? parseInt(id, 10) : 0;
    const [slideIndex, setSlideIndex] = useState<number>(0);
    const navigate = useNavigate();
    const UserPrivateDataResult = useGetUserPrivateQuery();
    const colonyQuestResult = useGetColonyEventQuery(idAsNumber);
    const [completeQuestMutation, completeQuestResult] = useCompleteEventMutation();
    const [inputTextValue, setInputTextValue] = useState('');
    const [inputTextError, setInputTextError] = useState('');
    const [handleChoiceError, setHandleChoiceError] = useState<string | undefined>(undefined);
    const [slideHistory, setSlideHistory] = useState<string[]>([]);

    const isLoading = UserPrivateDataResult.isLoading || colonyQuestResult.isLoading || completeQuestResult.isLoading;
    const error = UserPrivateDataResult.error ?? colonyQuestResult.error ?? completeQuestResult.error ?? handleChoiceError;

    const episode = colonyQuestResult.data?.data?.episode;
    const canBeClosed = colonyQuestResult.data?.data != undefined && colonyQuestResult.data.data.type != 'Autostart';
    const questCreatedAt = colonyQuestResult.data?.data?.createdAtUtc;

    useEffect(() => {
        if (!UserPrivateDataResult.isLoading && !UserPrivateDataResult.data?.data) {
            navigate('/registration');
        }
    }, [UserPrivateDataResult, navigate]);

    useEffect(() => {
        setSlideIndex(0);
        setSlideHistory([]);
        setInputTextValue('');
        setInputTextError('');
    }, [id]);

    const slides = episode?.slides;
    const currentSlide = slides?.[slideIndex] || slides?.[0];
    const eventResultSlide = completeQuestResult.data?.data;

    // ============================================
    // Логика
    // ============================================
    const handleSetSlideId = (slideId: string) => {
        if (!slides) return;
        const index = slides.findIndex(x => x.id === slideId);
        if (index !== -1) {
            setSlideHistory(prev => [...prev, currentSlide?.id || '']);
            setSlideIndex(index);
        }
    };

    const handleGoBack = () => {
        if (!slides || slideHistory.length === 0) return;
        const prevSlideId = slideHistory.pop();
        const index = slides.findIndex(x => x.id === prevSlideId);
        if (index !== -1) {
            setSlideIndex(index);
            setSlideHistory([...slideHistory]);
        }
    };

    const getDilemmaResolving = (action: SlideButtonAction, inputTextValue?: string) => {
        switch (action.type) {
            case 'inputCompleted':
                return inputTextValue!;
            case 'inputMissed':
                return '';
            case 'default':
            default:
                return action.arguments[1];
        }
    }

    const handleSetChoice = async (action: SlideButtonAction, inputTextValue?: string) => {
        try {
            const dilemmaResolving = getDilemmaResolving(action, inputTextValue);
            const result = await completeQuestMutation({
                colonyEventId: idAsNumber!,
                dilemmaResolving: dilemmaResolving
            }).unwrap();
            if (result.data == undefined || !result.data.show) {
                navigate('/me/colony');
            }
        } catch (e) {
            if (e && typeof e === 'object' && 'data' in e) {
                const errorData = (e as { data?: { title?: string } }).data;
                setHandleChoiceError(errorData?.title ?? 'Неизвестная ошибка.');
            } else {
                setHandleChoiceError('Неизвестная ошибка.');
            }
        }
    };

    const handleInputTextSave = async (action: SlideButtonAction) => {
        const sanitizedValue = SanitizeColonyName(inputTextValue);
        setInputTextValue(sanitizedValue);
        const validationResult = ValidateColonyName(sanitizedValue);
        if (!validationResult.isValid) {
            setInputTextError(validationResult.error!);
        } else {
            setInputTextError('');
            await handleSetChoice(action, sanitizedValue);
        }
    };

    const handleInputTextChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setInputTextValue(value);
        if (value.length > 1) {
            const validationResult = ValidateColonyName(value);
            setInputTextError(validationResult.isValid ? '' : validationResult.error!);
        } else {
            setInputTextError('');
        }
    };

    // ============================================
    // Обработчики для SlideRenderer
    // ============================================
    const handleButtonClick = (button: SlideButton) => {
        if (button.action) {
            if (button.action.type == 'inputCompleted') {
                handleInputTextSave(button.action);
            } else {
                handleSetChoice(button.action);
            }
        } else if (button.toSlide) {
            handleSetSlideId(button.toSlide.slideId);
        } else if (button.navigate) {
            navigate(button.navigate.actionUrl);
        }
    };

    const handleInfoSlideClick = (slideId: string) => {
        handleSetSlideId(slideId);
    };

    // ============================================
    // Рендер
    // ============================================

    const leftButton = slideHistory.length > 0
        ? { icon: ArrowLeft, onClick: () => handleGoBack(), label: 'Назад' }
        : undefined;
    const renderContent = () => {
        return eventResultSlide != undefined
            ? <ResultSlideRenderer
                eventResult={eventResultSlide!}
            />
            : <SlideRenderer
                slide={currentSlide!}
                inputTextValue={inputTextValue}
                inputTextError={inputTextError}
                onInputTextChange={handleInputTextChange}
                onButtonClick={handleButtonClick}
                onInfoSlideClick={handleInfoSlideClick}
                onSlideChange={handleSetSlideId}
                onNavigate={navigate}
                createdAt={questCreatedAt ? formatTimeAgo(questCreatedAt) : undefined}
                canBeClosed={canBeClosed}
                leftButton={leftButton}
                resetScrollTrigger={slideIndex}
            />
    };

    const backgroundImage = completeQuestResult.data != undefined
        ? 'captain_hall'
        : 'space'
    return (
        <Page backgroundImage={backgroundImage} darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default EventPage;