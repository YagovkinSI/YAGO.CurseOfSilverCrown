import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import StateList from '../shared/StateList';
import { useGetColonyRaitingQuery, type ColonyDetails } from '../entities/ColonyDetails';
import { type StateItem, StateItemStyles, GetGavernorTypeString, StateItemStyleType } from '../entities/StateItem';
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
        { type: 'Population', label: 'Население' },
        { type: 'ZonesOccupied', label: 'Занято секторов' }
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
                label = StateItemStyles(StateItemStyleType.Solars, 'Колония', 'Доход')
                break;
            case 'GavernorType':
                label = StateItemStyles(StateItemStyleType.Laws, 'Колония', 'Правитель')
                break;
            case 'Population':
                label = StateItemStyles(StateItemStyleType.Population, 'Колония', 'Население')
                break;
            case 'ZonesOccupied':
                label = StateItemStyles(StateItemStyleType.Zones, 'Колония', 'Занято секторов')
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
                    item = StateItemStyles(StateItemStyleType.Solars, colony.name, `${colony.colonyParameters.find(x => x.name == 'Economic_Budget_Balance')!.value}/ц`)
                    break;
                case 'GavernorType': {
                    const stringValue = GetGavernorTypeString(colony.colonyParameters.find(x => x.name == 'Mood_Total')!.value);
                    item = StateItemStyles(StateItemStyleType.Laws, colony.name, stringValue)
                    break; }
                case 'Population':
                    item = StateItemStyles(StateItemStyleType.Population, colony.name, `${colony.colonyParameters.find(x => x.name == 'Population_Total')!.value} чел.`)
                    break;
                case 'ZonesOccupied':
                    item = StateItemStyles(StateItemStyleType.Zones, colony.name, `${colony.colonyParameters.find(x => x.name == 'AreaCapacity_Occupied')!.value}`)
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