import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useGetUserPrivateQuery } from "../entities/users/user.api";
import { useGetColonyEventQuery } from "../entities/events/colonyEvent.api";
import useGameAction from '../features/UseGameAction';
import { SanitizeColonyName, ValidateColonyName } from '../features/ColonyNameValidator';
import Page from '../widgets/Page';
import SlideRenderer from '../widgets/SlideRenderer';
import { ArrowLeft } from 'lucide-react';
import ResultSlideRenderer from '../entities/events/ResultSlideRenderer';
import type { SlideButton } from '../entities/events/colonyEvent.types';

const EventPage: React.FC = () => {
    const { id } = useParams();
    const idAsNumber = id ? parseInt(id, 10) : 0;
    const [slideIndex, setSlideIndex] = useState<number>(0);
    const navigate = useNavigate();
    const UserPrivateDataResult = useGetUserPrivateQuery();
    const colonyQuestResult = useGetColonyEventQuery(idAsNumber);
    const action = useGameAction();
    const [inputTextValue, setInputTextValue] = useState('');
    const [inputTextError, setInputTextError] = useState('');
    const [slideHistory, setSlideHistory] = useState<string[]>([]);

    const isLoading = UserPrivateDataResult.isLoading || colonyQuestResult.isLoading || action.isLoading;
    const error = UserPrivateDataResult.error ?? colonyQuestResult.error ?? action.error;

    const episode = colonyQuestResult.data?.data?.episode;
    const canBeClosed = colonyQuestResult.data?.data != undefined && colonyQuestResult.data.data.type != 'Autostart';

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
    const eventResultSlide = action.data?.data;

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

    const handleInputTextSave = async (button: SlideButton) => {
        const sanitizedValue = SanitizeColonyName(inputTextValue);
        setInputTextValue(sanitizedValue);
        const validationResult = ValidateColonyName(sanitizedValue);
        if (!validationResult.isValid) {
            setInputTextError(validationResult.error!);
        } else {
            setInputTextError('');
            await action.apply(button, sanitizedValue);
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
        if (!button.action) return;
        if (button.action.needsInput) {
            handleInputTextSave(button);
        } else {
            action.apply(button);
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
                actions={{
                    onButtonClick: handleButtonClick,
                    onInfoSlideClick: handleInfoSlideClick,
                    onSlideChange: handleSetSlideId,
                }}
                inputState={{
                    value: inputTextValue,
                    error: inputTextError,
                    onChange: handleInputTextChange,
                }}
                header={{ leftButton: leftButton, canBeClosed: canBeClosed }}
                resetScrollTrigger={slideIndex}
            />
    };

    const backgroundImage = action.data != undefined
        ? 'captain_hall'
        : 'space'
    return (
        <Page backgroundImage={backgroundImage} darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default EventPage;