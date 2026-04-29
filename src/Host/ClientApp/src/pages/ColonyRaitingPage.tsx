import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import StateList from '../shared/StateList';
import { useGetColonyRaitingQuery, type ColonyDetails } from '../entities/ColonyDetails';
import { type StateItem, StateItemStyles } from '../entities/StateItem';
import { FormatListNumbered, WorkspacePremium } from '@mui/icons-material';
import YagoCardContentSelection from '../shared/YagoCardContentSelection';

const ColonyRaitingPage: React.FC = () => {
    const navigate = useNavigate();

    const colonyRaitingResult = useGetColonyRaitingQuery({ page: 1 });

    const isLoading = colonyRaitingResult.isLoading;
    const error = colonyRaitingResult.error;

    const raitingTypes = [
        { type: 'Population', label: 'Население' },
        { type: 'GavernorType', label: 'Законы' },
        { type: 'Mood', label: 'Доверие' },
        { type: 'SolarIncome', label: 'Бюджет' },
        { type: 'Attractiveness_Total', label: 'Привлекательность' },
        { type: 'ZonesOccupied', label: 'Занято секторов' },
        { type: 'EpisodeCount', label: 'Сделано ходов' },
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
                label = StateItemStyles("Economic", 'Колония', 'Бюджет')
                break;
            case 'GavernorType':
                label = StateItemStyles("Laws_CodeOfLaws", 'Колония', 'Законы')
                break;
            case 'Mood':
                label = StateItemStyles("Mood_Total", 'Колония', 'Доверие')
                break;
            case 'Population':
                label = StateItemStyles("Population_Total", 'Колония', 'Население')
                break;
            case 'ZonesOccupied':
                label = StateItemStyles("AreaCapacity", 'Колония', 'Занято секторов')
                break;
            case 'EpisodeCount':
                label = StateItemStyles("EpisodeCount", 'Колония', 'Сделано ходов')
                break;
            case 'Attractiveness_Total':
                label = StateItemStyles("Attractiveness_Total", 'Колония', 'Привлекательность')
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
                    item = StateItemStyles("Economic", colony.name, `${colony.colonyParameters.find(x => x.type == 'Economic')?.value ?? 0}`)
                    break;
                case 'GavernorType': {
                    item = StateItemStyles("Laws_CodeOfLaws", colony.name, colony.colonyParameters.find(x => x.type == 'Laws_CodeOfLaws')?.value ?? 'Не определены')
                    break; }
                case 'Mood': {
                    item = StateItemStyles("Mood_Total", colony.name, `${colony.colonyParameters.find(x => x.type == 'Mood_Total')?.value ?? 'Не определено'}`)
                    break; }
                case 'Population':
                    item = StateItemStyles("Population_Total", colony.name, `${colony.colonyParameters.find(x => x.type == 'Population_Total')?.value ?? 0} чел.`)
                    break;
                case 'ZonesOccupied':
                    item = StateItemStyles("AreaCapacity", colony.name, `${colony.colonyParameters.find(x => x.type == 'AreaCapacity')?.value ?? 0}`)
                    break;
                case 'EpisodeCount':
                    item = StateItemStyles("EpisodeCount", colony.name, `${colony.colonyParameters.find(x => x.type == 'EpisodeCount')?.value ?? 0}`)
                    break;
                case 'Attractiveness_Total':
                    item = StateItemStyles("Attractiveness_Total", colony.name,  `${colony.colonyParameters.find(x => x.type == 'Attractiveness_Total')?.value ?? 'Не определено'}`)
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
                <YagoButton onClick={() => navigate(-1)} type='secondary'>Закрыть</YagoButton>
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