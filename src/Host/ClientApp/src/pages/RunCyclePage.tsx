import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import isErrorWithStatus from '../shared/ErrorHandler';
import TextMain from '../shared/TextMain';
import { useRunCycleMutation, useSetChoiceMutation } from '../entities/MyCycle';
import type { Dilemma, DilemmaSelect, DilemmaTextInput, Episode, PrologueSlide } from "../entities/Episode";
import YagoCardContentSelection from '../shared/YagoCardContentSelection';
import type { ColonyParameter } from '../entities/ColonyParameter';
import { useGetMyColonyQuery } from '../entities/MyColony';
import ColonyParameterList from '../features/ColonyParameterList';

const RunCyclePage: React.FC = () => {
    const [slideIndex, setSlideIndex] = useState<number>(0);
    const myColonyResult = useGetMyColonyQuery();
    const [runCycleMutation, runCycleResult] = useRunCycleMutation();
    const [setChoiceMutation] = useSetChoiceMutation();
    const navigate = useNavigate();
    const [choiceIndex, setChoiceIndex] = useState<number>(0);
    const [handleChoiceError, setHandleChoiceError] = useState<string | undefined>(undefined);

    const isLoading = runCycleResult.isLoading;
    const error = runCycleResult.error ?? handleChoiceError;

    const colony = myColonyResult?.data?.data;
    const episode = runCycleResult?.data?.data;
    const title = episode?.title ?? "Мир YAGO";
    const dilemma = episode?.dilemma;
    const hasChoce = dilemma != undefined;
    const slideCount = (episode?.prologueSlides.length ?? 0) + (hasChoce ? 1 : 0);

    useEffect(() => {
        runCycleMutation();
    }, [runCycleMutation]);

    useEffect(() => {
        if (error != undefined && isErrorWithStatus(error, 401))
            navigate('/registration');
    }, [error, navigate]);

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const handleNextChoice = (dilemma: DilemmaSelect) => {
        const nextIndex = (choiceIndex + 1) % dilemma.choice.length;
        setChoiceIndex(nextIndex);
    };

    const handlePrevChoice = (dilemma: DilemmaSelect) => {
        const prevIndex = (choiceIndex - 1 + dilemma.choice.length) % dilemma.choice.length;
        setChoiceIndex(prevIndex);
    };

    const handleDilemmaResolving = async (dilemmaResolving: string) => {
        try {
            await setChoiceMutation({ dilemmaResolving: dilemmaResolving }).unwrap();
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

    const renderParameters = (parameters: ColonyParameter[]) => {
        if (parameters.length == 0)
            return <></>

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
                <ColonyParameterList items={parameters} />
            </Box>
        )
    }

    const renderPrologueSlide = (slide: PrologueSlide, isCycleCompleted: boolean) => {
        return (
            <YagoCard
                title={title}
                image={`/assets/images/pictures/${slide.imageName}.jpg`}
            >
                <TextMain textArray={slide.text} />
                {renderParameters(slide.parameters)}
                {slideIndex > 0 && <YagoButton onClick={() => setSlideIndex(slideIndex - 1)} type='secondary'>Назад</YagoButton>}
                {slideIndex < slideCount - 1 && <YagoButton onClick={() => setSlideIndex(slideIndex + 1)}>{slide.continueButtonName}</YagoButton>}
                {slideIndex == slideCount - 1 && !hasChoce && !isCycleCompleted && <YagoButton onClick={() => runCycleMutation().unwrap()}>{slide.continueButtonName}</YagoButton>}
                {renderCloseButton()}
            </YagoCard>
        )
    }

    function isDilemmaSelect(dilemma: Dilemma): dilemma is DilemmaSelect {
        return dilemma.dilemmaType === "Select";
    }

    function isDilemmaTextInput(dilemma: Dilemma): dilemma is DilemmaTextInput {
        return dilemma.dilemmaType === "TextInput";
    }

    const renderCloseButton = () => {
        const path = colony?.autoRunCycle ? "/" : "/me/colony";
        return (
            <YagoButton onClick={() => navigate(path)} type='secondary'>Закрыть</YagoButton>
        )
    } 

    const renderDilemmaSelectSlide = (dilemma: DilemmaSelect) => {
        const choiceSlides = dilemma.choice;
        const currentChoice = choiceSlides[choiceIndex];

        return (
            <YagoCard
                title={title}
                image={`/assets/images/pictures/${currentChoice.imageName}.jpg`}
            >
                <TextMain textArray={dilemma.choiceLabel} sx={{ textAlign: 'center' }} />
                <YagoCardContentSelection handlePrev={() => handlePrevChoice(dilemma)} label={currentChoice.title} handleNext={() => handleNextChoice(dilemma)} />
                <TextMain textArray={currentChoice.text} />
                {renderParameters(currentChoice.parameters)}
                <YagoButton onClick={() => setSlideIndex(slideIndex - 1)} type='secondary'>Назад</YagoButton>
                <YagoButton onClick={() => handleDilemmaResolving(currentChoice.id)} isDisabled={!currentChoice.isAvailable}>{currentChoice.continueButtonName}</YagoButton>
                {renderCloseButton()}
            </YagoCard>
        )
    }

    const renderDilemmaDilemmaTextInputSlide = (dilemma: DilemmaTextInput) => {
        const slide = dilemma.slide;

        return (
            <YagoCard
                title={title}
                image={`/assets/images/pictures/${slide.imageName}.jpg`}
            >
                <TextMain textArray={slide.text} />
                {renderParameters(slide.parameters)}
                <YagoButton onClick={() => setSlideIndex(slideIndex - 1)} type='secondary'>Назад</YagoButton>
                {renderCloseButton()}
            </YagoCard>
        )
    }

    const renderDilemmaSlide = (dilemma: Dilemma) => {
        if (isDilemmaSelect(dilemma))
            return renderDilemmaSelectSlide(dilemma);
        else if (isDilemmaTextInput(dilemma))
            return renderDilemmaDilemmaTextInputSlide(dilemma);
    }

    const renderCard = (episode: Episode) => {
        const isPrologStep = slideIndex < episode.prologueSlides.length;
        if (isPrologStep || episode.dilemma == null)
            return renderPrologueSlide(episode.prologueSlides[slideIndex], episode.isCycleCompleted);
        return renderDilemmaSlide(episode.dilemma);
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