import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React, { useEffect, useState } from 'react';
import StateList from '../shared/StateList';
import { GetStateItems } from '../entities/StateItem';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import isErrorWithStatus from '../shared/ErrorHandler';
import TextMain from '../shared/TextMain';
import { useRunCycleMutation, useSetChoiceMutation } from '../entities/MyCycle';
import type { Dilemma, Episode, Slide } from "../entities/Episode";
import YagoCardContentSelection from '../shared/YagoCardContentSelection';
import YagoCardContentInputField from '../shared/YagoCardContentInputField';

const RunCyclePage: React.FC = () => {
    const [slideIndex, setSlideIndex] = useState<number>(0);
    const [runCycleMutation, runCycleResult] = useRunCycleMutation();
    const [setChoiceMutation] = useSetChoiceMutation();
    const navigate = useNavigate();
    const [choiceIndex, setChoiceIndex] = useState<number>(0);
    const [handleChoiceError, setHandleChoiceError] = useState<string | undefined>(undefined);
    const [inputTextValue, setInputTextValue] = useState('');
    const [inputTextError] = useState('');

    const isLoading = runCycleResult.isLoading;
    const error = runCycleResult.error ?? handleChoiceError;
    const episode = runCycleResult?.data?.data;
    const dilemma = episode?.dilemma;
    const hasChoce = (dilemma?.choice.length ?? 0) > 0
    const slideCount = (episode?.prologSlides.length ?? 0) + (hasChoce ? 1 : 0);

    useEffect(() => {
        runCycleMutation();
    }, [runCycleMutation]);

    useEffect(() => {
        if (error != undefined && isErrorWithStatus(error, 401))
            navigate('/registration');
    }, [error, navigate]);

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const handleNextChoice = (dilemma: Dilemma) => {
        const nextIndex = (choiceIndex - 1 + dilemma.choice.length) % dilemma.choice.length;
        setChoiceIndex(nextIndex);
    };

    const handlePrevChoice = (dilemma: Dilemma) => {
        const prevIndex = (choiceIndex + 1) % dilemma.choice.length;
        setChoiceIndex(prevIndex);
    };

    const handleInputTextChange = (e: React.ChangeEvent<HTMLInputElement>) => {
            const value = e.target.value;
            setInputTextValue(value);
        };

    const handleChoice = async (choiceId: string) => {
        try {
            await setChoiceMutation({ choiceId: choiceId }).unwrap();
            navigate('/me/colony');
        } catch (e) {
            if (e && typeof e === 'object' && 'data' in e) {
                const errorData = (e as { data?: { title?: string } }).data;
                setHandleChoiceError(errorData?.title ?? 'Неизвестная ошибка.');
            } else {
                setHandleChoiceError('Неизвестная ошибка.');
            }
        }
    };

    const renderParameters = (slide: Slide) => {
        if (slide.parameters.length == 0)
            return <></>

        const stats = GetStateItems(slide.parameters, true);

        return (
            <Box
                display="flex"
                flexDirection="column"
                gap={1}
                sx={{
                    width: '100%',
                    maxWidth: isMobile ? 350 : 700,
                    margin: '0 auto'
                }}
            >
                <StateList items={stats} />
            </Box>
        )
    }

    const renderPrologueSlide = (slide: Slide, isCycleCompleted: boolean) => {
        return (
            <YagoCard
                title={slide.title}
                image={`/assets/images/pictures/${slide.imageName}.jpg`}
            >
                <TextMain textArray={slide.text} />
                {renderParameters(slide)}
                {slideIndex > 0 && <YagoButton onClick={() => setSlideIndex(slideIndex - 1)} type='secondary'>Назад</YagoButton>}
                {slideIndex < slideCount - 1 && <YagoButton onClick={() => setSlideIndex(slideIndex + 1)}>{slide.buttonName}</YagoButton>}
                {slideIndex == slideCount - 1 && !hasChoce && !isCycleCompleted && <YagoButton onClick={() => runCycleMutation().unwrap()}>{slide.buttonName}</YagoButton>}
                <YagoButton onClick={() => navigate("/me/colony")} type='secondary'>Закрыть</YagoButton>
            </YagoCard>
        )
    }

    const renderDilemmaSlide = (dilemma: Dilemma, title: string) => {
        const choiceSlides = dilemma.choice;
        const currentChoice = choiceSlides[choiceIndex];

        return (
            <YagoCard
                title={title}
                image={`/assets/images/pictures/${currentChoice.imageName}.jpg`}
            >
                <TextMain textArray={dilemma.choiceLabel} sx={{ textAlign: dilemma.choiceType == 'Select' ? 'center' : 'justify' }} />
                {dilemma.choiceType == 'Select' && <YagoCardContentSelection handlePrev={() => handlePrevChoice(dilemma)} label={currentChoice.title} handleNext={() => handleNextChoice(dilemma)} />}
                {dilemma.choiceType == 'TextInput' && <YagoCardContentInputField value={inputTextValue} label='Название колонии' handleChange={handleInputTextChange} error={inputTextError} />}
                <TextMain textArray={currentChoice.text} />
                {renderParameters(currentChoice)}
                <YagoButton onClick={() => setSlideIndex(slideIndex - 1)} type='secondary'>Назад</YagoButton>
                <YagoButton onClick={() => handleChoice(currentChoice.id)} isDisabled={!currentChoice.isAvailable}>{currentChoice.buttonName}</YagoButton>
                <YagoButton onClick={() => navigate("/me/colony")} type='secondary'>Закрыть</YagoButton>
            </YagoCard>
        )
    }

    const renderCard = (episode: Episode) => {
        const isPrologStep = slideIndex < episode.prologSlides.length;
        if (isPrologStep || episode.dilemma == null)
            return renderPrologueSlide(episode.prologSlides[slideIndex], episode.isCycleCompleted);
        return renderDilemmaSlide(episode.dilemma, episode.prologSlides[0].title);
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading || episode == undefined
                ? <LoadingCard />
                : error != undefined
                    ? <DefaultErrorCard />
                    : renderCard(episode)}
        </>
    )
}

export default RunCyclePage