import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React, { useEffect } from 'react';
import StateList from '../shared/StateList';
import { GetStateItems } from '../entities/StateItem';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import isErrorWithStatus from '../shared/ErrorHandler';
import { useRunCycleMutation } from '../entities/ColonyActions';
import TextMain from '../shared/TextMain';
import { CycleState } from '../entities/MyCycle';

const RunCyclePage: React.FC = () => {
    const [runCycleMutation, runCycleResult] = useRunCycleMutation();

    const isLoading = runCycleResult.isLoading;
    const error = runCycleResult.error;

    const navigate = useNavigate();
    React.useEffect(() => {
        runCycleMutation({});
    }, [runCycleMutation]);

    useEffect(() => {
        if (error != undefined && isErrorWithStatus(error, 401))
            navigate('/registration');
    }, [error, navigate]);

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const renderText = () => {
        return (
            <TextMain textArray={runCycleResult.data?.episode?.slides[0]?.text ?? ['-']} />
        )
    }

    const renderParameters = () => {
        if (runCycleResult.data?.episode?.slides[0]?.parameters == undefined)
            return <></>

        const stats = GetStateItems(runCycleResult.data!.episode!.slides[0]!.parameters, true);

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

    const renderButton = () => {
        const cycleCompleted = runCycleResult.data?.updatedEntities.myCycle?.state != CycleState.InProgress;
        return (
            <>
                {!cycleCompleted && <YagoButton variant='contained' onClick={() => runCycleMutation({}).unwrap()} text={"Далее"} />}
                <YagoButton variant='outlined' onClick={() => navigate("/me/colony")} text={"Закрыть"} />
            </>
        );
    }

    const renderCard = () => {
        return (
            <YagoCard
                title={runCycleResult.data?.episode?.slides[0]?.title ?? '-'}
                image={`/assets/images/pictures/${runCycleResult.data?.episode?.slides[0]?.illustration ?? 'RegularCycle'}.jpg`}
            >
                {renderText()}
                {renderParameters()}
                {renderButton()}
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading
                ? <LoadingCard />
                : error != undefined
                    ? <DefaultErrorCard />
                    : renderCard()}
        </>
    )
}

export default RunCyclePage