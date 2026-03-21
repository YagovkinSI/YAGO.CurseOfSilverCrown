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
import { useRunCycleMutation } from '../entities/MyCycle';
import type { Episode } from "../entities/Episode";

const RunCyclePage: React.FC = () => {
    const [slideIndex, setSlideIndex] = useState<number>(0);
    const [runCycleMutation, runCycleResult] = useRunCycleMutation();
    const navigate = useNavigate();

    const isLoading = runCycleResult.isLoading;
    const error = runCycleResult.error;
    const episode = runCycleResult?.data?.data;

    useEffect(() => {
        runCycleMutation();
    }, [runCycleMutation]);

    useEffect(() => {
        if (error != undefined && isErrorWithStatus(error, 401))
            navigate('/registration');
    }, [error, navigate]);

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const renderText = (episode: Episode) => {
        return (
            <TextMain textArray={episode.slides[0].text} />
        )
    }

    const renderParameters = (episode: Episode) => {
        if (episode.slides[0].parameters.length == 0)
            return <></>

        const stats = GetStateItems(episode.slides[0].parameters, true);

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

    const renderButtons = (cycleIsComplete: boolean) => {
        return (
            <>
                {slideIndex > 0 && <YagoButton variant='outlined' onClick={() => setSlideIndex(slideIndex - 1)} text={"Назад"} />}
                {!cycleIsComplete && <YagoButton variant='contained' onClick={() => runCycleMutation().unwrap()} text={"Далее"} />}
                <YagoButton variant='outlined' onClick={() => navigate("/me/colony")} text={"Закрыть"} />
            </>
        );
    }

    const renderCard = (episode: Episode) => {
        return (
            <YagoCard
                title={episode.slides[0].title}
                image={`/assets/images/pictures/${episode.slides[0].imageName}.jpg`}
            >
                {renderText(episode)}
                {renderParameters(episode)}
                {renderButtons(episode.isCycleCompleted)}
            </YagoCard>
        )
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