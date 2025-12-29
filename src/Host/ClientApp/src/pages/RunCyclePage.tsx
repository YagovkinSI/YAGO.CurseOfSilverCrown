import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React, { useEffect } from 'react';
import StateList from '../shared/StateList';
import { StateItemSolar, type StateItem } from '../entities/StateItem';
import { useNavigate } from 'react-router-dom';
import YagoButton from '../shared/YagoButton';
import isErrorWithStatus from '../shared/ErrorHandler';
import { ColonyParameterType, useRunCycleMutation } from '../entities/ColonyActions';
import TextMain from '../shared/TextMain';

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

    const renderText = () => {
        return (
            <TextMain textArray={runCycleResult.data?.notification?.text ?? ['-']} />
        )
    }

    const stats: StateItem[] = [
        StateItemSolar(
            'Солары',
            runCycleResult.data?.notification?.parameters.find(x => x.type == ColonyParameterType.Solars)?.value ?? 0),
    ];

    const renderCloseButton = () => {
        return (
            <YagoButton variant='outlined' onClick={() => navigate("/me/colony")} text={"Закрыть"} />
        );
    }

    const renderCard = () => {
        return (
            <YagoCard
                title={runCycleResult.data?.notification?.title ?? '-'}
                image={`/assets/images/pictures/runCycle/${runCycleResult.data?.notification?.illustration ?? 'RegularCycle'}.jpg`}
            >
                {renderText()}
                <StateList items={stats} />
                {renderCloseButton()}
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