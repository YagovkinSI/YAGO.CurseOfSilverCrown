import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import StateList from '../shared/StateList';
import { useGetColonyRaitingQuery, type ColonyDetails } from '../entities/ColonyDetails';
import { StateItemPopulation, StateItemGavernorType, StateItemSolar, type StateItem } from '../entities/StateItem';
import { FormatListNumbered, WorkspacePremium } from '@mui/icons-material';
import YagoCardContentSelection from '../shared/YagoCardContentSelection';

const ColonyRaitingPage: React.FC = () => {
    const navigate = useNavigate();

    const colonyRaitingResult = useGetColonyRaitingQuery({ page: 1 });

    const isLoading = colonyRaitingResult.isLoading;
    const error = colonyRaitingResult.error;

    const raitingTypes = [
        { type: 'SolarIncome', label: 'Доход' },
        { type: 'GavernorType', label: 'Правители' },
        { type: 'Population', label: 'Население' }
    ];

    const [raitingTypeIndex, setRaitingTypeIndex] = useState<number>(0);

    const handleNextRaiting = () => {
        const nextIndex = (raitingTypeIndex + 1) % raitingTypes.length;
        setRaitingTypeIndex(nextIndex);
    };

    const handlePrevRaiting = () => {
        const prevIndex = (raitingTypeIndex - 1 + raitingTypes.length) % raitingTypes.length;
        setRaitingTypeIndex(prevIndex);
    };


    const getRaitingLabel = (raitingType: string): StateItem => {
        let label;
        switch (raitingType) {
            case 'SolarIncome':
                label = StateItemSolar('Колония', 'Доход')
                break;
            case 'GavernorType':
                label = StateItemGavernorType('Колония', 'Правитель')
                break;
            case 'Population':
                label = StateItemPopulation('Колония', 'Население')
                break;
        }
        label!.icon = FormatListNumbered;
        return label!;
    };

    const getRaitingItems = (data: ColonyDetails[], raitingType: string): StateItem[] => {

        return data.map(colony => {
            let item;
            switch (raitingType) {
                case 'SolarIncome':
                    item = StateItemSolar(colony.name, `${colony.solarsIncome}/ц`)
                    break;
                case 'GavernorType':
                    item = StateItemGavernorType(colony.name, colony.gavernorType)
                    break;
                case 'Population':
                    item = StateItemPopulation(colony.name, `${colony.population} чел.`)
                    break;
            }
            item!.icon = WorkspacePremium;

            return item!;
        })
    };

    const renderCard = (data: ColonyDetails[]) => {
        const raitingStats: StateItem[] = [
            getRaitingLabel(raitingTypes[raitingTypeIndex].type),
            ...getRaitingItems(data, raitingTypes[raitingTypeIndex].type)
        ];

        return (
            <YagoCard
                title={'Колонии'}
                image={undefined}
            >
                <YagoCardContentSelection handlePrev={handlePrevRaiting} label={raitingTypes[raitingTypeIndex].label} handleNext={handleNextRaiting} />
                <StateList items={raitingStats} />
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