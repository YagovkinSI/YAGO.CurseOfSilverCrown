import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import StateList from '../shared/StateList';
import { useGetColonyRaitingQuery, type ColonyDetails } from '../entities/ColonyDetails';
import type { StateItem } from '../entities/StateItem';
import { WorkspacePremium } from '@mui/icons-material';

const ColonyRaitingPage: React.FC = () => {
    const navigate = useNavigate();

    const colonyRaitingResult = useGetColonyRaitingQuery({ page: 1 });

    const isLoading = colonyRaitingResult.isLoading;
    const error = colonyRaitingResult.error;

    const renderCard = (data: ColonyDetails[]) => {
        const stats: StateItem[] = data.map(colony => {
            return {
                icon: WorkspacePremium,
                label: colony.name,
                value: colony.solarsIncome,
                color: '#FFD700'
            }})

        return (
            <YagoCard
                title={'Колонии'}
                image={undefined}
            >
                <StateList items={stats} />
                <YagoButton onClick={() => navigate(-1)} text={'Закрыть'} isDisabled={false} />
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading
                ? <LoadingCard />
                : error != undefined || colonyRaitingResult.data?.data == undefined
                    ? <DefaultErrorCard />
                    : renderCard(colonyRaitingResult.data.data)}
        </>
    )
}

export default ColonyRaitingPage