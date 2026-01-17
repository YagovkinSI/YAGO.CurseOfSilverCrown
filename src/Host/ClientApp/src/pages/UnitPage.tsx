import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { StateItemPopulation, StateItemGavernorType, StateItemSolar, StateItemZones, type StateItem } from '../entities/StateItem';
import StateList from '../shared/StateList';
import type { Slide } from '../entities/Slide';
import SlideCard from '../features/SlideCard';
import { useGetMyColonyQuery } from '../entities/MyColony';
import isErrorWithStatus from '../shared/ErrorHandler';
import { useGetUnitQuery, type UnitDetails } from '../entities/UnitDetails';
import YagoCardContentSelection from '../shared/YagoCardContentSelection';
import { useHireUnitMutation } from '../entities/ColonyActions';
import TextMain from '../shared/TextMain';

const UnitPage: React.FC = () => {
    const navigate = useNavigate();

    const [unitId, setUnitId] = useState<number>(1);
    const unitIdMax = 3;
    const unitResult = useGetUnitQuery(unitId);

    const myColonyResult = useGetMyColonyQuery();
    const [hireUnit, useHireUnitResult] = useHireUnitMutation();

    const [showSlide, setShowSlide] = useState<boolean>(false);

    const isLoading = unitResult.isLoading || myColonyResult.isLoading || useHireUnitResult.isLoading;
    const error = unitResult.error ?? myColonyResult.error ?? useHireUnitResult.error;

    useEffect(() => {
        if (error != undefined && isErrorWithStatus(error, 401))
            navigate('/registration');
    }, [error, navigate]);

    const handleNextUnit = () => {
        const nextIndex = unitId % unitIdMax + 1;
        setUnitId(nextIndex);
    };

    const handlePrevUnit = () => {
        const prevIndex = unitId == 1 ? unitIdMax : unitId - 1;
        setUnitId(prevIndex);
    };

    const handleHireUnit = async (unitId: number) => {
        await hireUnit({ unitId: unitId }).unwrap();
        navigate('/me/colony');
    }

    const stats = (unit: UnitDetails): StateItem[] => {
        return [
            StateItemSolar('Цена', `${unit.cost}`),
            StateItemZones('Сектора', `${unit.zonesOccupied}`),
            StateItemSolar('Доход', `+${unit.solarsIncome}/ц`),
            StateItemGavernorType('Путь', `${unit.gavernorType}`),
            StateItemPopulation('Население', `+${unit.population} чел.`),
        ]
    };

    const renderSlideCard = (unit: UnitDetails) => {
        const slide: Slide = {
            id: unit.id,
            title: unit.name,
            imageName: `buildings/${unit.id}`,
            text: unit.description,
            footer: undefined
        };

        return (
            <SlideCard slide={slide} closeAction={() => setShowSlide(false)} />
        )
    }

    const renderCard = (unit: UnitDetails) => {
        const isActive = myColonyResult.data?.isAuthorized
            && myColonyResult.data.data != undefined
            && myColonyResult.data.data.solars > unit.cost
            && myColonyResult.data.data.zonesTotal - myColonyResult.data.data.zonesOccupied >= unit.zonesOccupied

        return (
            <YagoCard
                title='Найм'
                image={`/assets/images/buildings/${unit.id ?? '2'}.jpg`}
            >
                <YagoCardContentSelection handlePrev={handlePrevUnit} label={unit.name} handleNext={handleNextUnit} />
                <TextMain textArray={unit.text} sx={{ textAlign: 'justify' }} />
                <StateList items={stats(unit)} />
                <YagoButton onClick={() => navigate(-1)} text={'Закрыть'} isDisabled={false} />
                <YagoButton variant="contained" onClick={() => handleHireUnit(unit.id)} text={'Купить'} isDisabled={!isActive} />
                <YagoButton onClick={() => setShowSlide(true)} text={'Описание'} />
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading || unitResult.data == undefined
                ? <LoadingCard />
                : error != undefined
                    ? <DefaultErrorCard />
                    : showSlide
                        ? renderSlideCard(unitResult.data)
                        : renderCard(unitResult.data)}
        </>
    )
}

export default UnitPage