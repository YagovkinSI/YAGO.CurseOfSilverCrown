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
import type { Episode } from "../entities/Episode";
import type { Slide } from '../entities/Slide';
import YagoCardContentSelection from '../shared/YagoCardContentSelection';

const RunCyclePage: React.FC = () => {
    const [slideIndex, setSlideIndex] = useState<number>(0);
    const [runCycleMutation, runCycleResult] = useRunCycleMutation();
    const [setChoiceMutation] = useSetChoiceMutation();
    const navigate = useNavigate();
    const [choiceIndex, setChoiceIndex] = useState<number>(0);

    const isLoading = runCycleResult.isLoading;
    const error = runCycleResult.error;
    const episode = runCycleResult?.data?.data;
    const slideCount = (episode?.prologSlides.length ?? 0) + 1;

    useEffect(() => {
        runCycleMutation();
    }, [runCycleMutation]);

    useEffect(() => {
        if (error != undefined && isErrorWithStatus(error, 401))
            navigate('/registration');
    }, [error, navigate]);

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const handleNextChoice = (episode: Episode) => {
        const nextIndex = (choiceIndex - 1 + episode.choice.length) % episode.choice.length;
        setChoiceIndex(nextIndex);
    };

    const handlePrevChoice = (episode: Episode) => {
        const prevIndex = (choiceIndex + 1) % episode.choice.length;
        setChoiceIndex(prevIndex);
    };

    const handleChoice = async (choiceId: string) => {
        await setChoiceMutation({ choiceId: choiceId }).unwrap();
        navigate('/me/colony');
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

    const renderSimpleSlide = (slide: Slide, isCycleCompleted: boolean) => {
        return (
            <YagoCard
                title={slide.title}
                image={`/assets/images/pictures/${slide.imageName}.jpg`}
            >
                <TextMain textArray={slide.text} />
                {renderParameters(slide)}
                <YagoButton variant='outlined' onClick={() => navigate("/me/colony")} text={"Закрыть"} />
                {slideIndex > 0 && <YagoButton variant='outlined' onClick={() => setSlideIndex(slideIndex - 1)} text={"Назад"} />}
                {slideIndex < slideCount - 1 && <YagoButton variant='outlined' onClick={() => setSlideIndex(slideIndex + 1)} text={"Далее"} />}
                {slideIndex == slideCount - 1 && !isCycleCompleted && <YagoButton variant='contained' onClick={() => runCycleMutation().unwrap()} text={"Далее"} />}
            </YagoCard>
        )
    }

    const renderChoiceSlide = (choiceSlides: Slide[], episode: Episode) => {
        const currentChoice = choiceSlides[choiceIndex];

        return (
            <YagoCard
                title={episode.prologSlides[0].title}
                image={`/assets/images/pictures/${currentChoice.imageName}.jpg`}
            >
                <TextMain textArray={[episode.choiceLabel ?? 'Сделай выбор']} sx={{ textAlign: 'center' }} />
                <YagoCardContentSelection handlePrev={() => handlePrevChoice(episode)} label={currentChoice.title} handleNext={() => handleNextChoice(episode)} />
                <TextMain textArray={currentChoice.text} />
                {renderParameters(currentChoice)}
                <YagoButton variant='outlined' onClick={() => navigate("/me/colony")} text={"Закрыть"} />
                <YagoButton onClick={() => setSlideIndex(slideIndex - 1)} text={'Назад'} isDisabled={false} />
                <YagoButton variant='contained' onClick={() => handleChoice(currentChoice.id)} text={"Выбрать"} />
            </YagoCard>
        )
    }

    const renderCard = (episode: Episode) => {
        const isChoiceStep = slideIndex == episode.prologSlides.length;
        if (!isChoiceStep)
            return renderSimpleSlide(episode.prologSlides[slideIndex], episode.isCycleCompleted);
        const isChoice = isChoiceStep && episode.choice.length > 1;
        if (!isChoice)
            return renderSimpleSlide(episode.choice[0], episode.isCycleCompleted);
        else
            return renderChoiceSlide(episode.choice, episode);
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