import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import YagoButton from '../shared/YagoButton';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { GetStateItems } from '../entities/StateItem';
import StateList from '../shared/StateList';
import SlideCard from '../features/SlideCard';
import { useGetMyColonyQuery, useIssueDecreeMutation } from '../entities/MyColony';
import { useGetDecreeQuery, type DecreeDetails } from '../entities/DecreeDetails';
import YagoCardContentSelection from '../shared/YagoCardContentSelection';
import TextMain from '../shared/TextMain';
import type { Slide } from '../entities/Episode';

const DecreePage: React.FC = () => {
    const [decreeId, setDecreeId] = useState<number>(1);
    const [showSlide, setShowSlide] = useState<boolean>(false);
    const myColonyResult = useGetMyColonyQuery();
    const decreeResult = useGetDecreeQuery(decreeId);
    const [issueDecree, issueDecreeResult] = useIssueDecreeMutation();
    const navigate = useNavigate();

    const isLoading = decreeResult.isLoading || myColonyResult.isLoading || issueDecreeResult.isLoading;
    const error = decreeResult.error ?? myColonyResult.error ?? issueDecreeResult.error;
    const decreeIdMax = 3;

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data.data == undefined)
            navigate('/createColony');
    }, [myColonyResult, navigate]);

    const handleNextDecree = () => {
        const nextIndex = decreeId % decreeIdMax + 1;
        setDecreeId(nextIndex);
    };

    const handlePrevDecree = () => {
        const prevIndex = decreeId == 1 ? decreeIdMax : decreeId - 1;
        setDecreeId(prevIndex);
    };

    const handleIssueDecree = async (decreeId: number) => {
        await issueDecree({ decreeId }).unwrap();
        navigate('/me/colony');
    }

    const renderSlideCard = (decree: DecreeDetails) => {
        const slide: Slide = {
            title: decree.name,
            imageName: `pictures/${decree.image}`,
            text: decree.description,
            parameters: [],
            footer: undefined
        };
        return (
            <SlideCard slide={slide} closeAction={() => setShowSlide(false)} />
        )
    }

    const validateDecree = (decree: DecreeDetails): { isActive: boolean, buttonName: string } => {
        if (myColonyResult.data?.data == undefined)
            return { isActive: false, buttonName: 'Создайте колонию' }

        if ((myColonyResult.data.data.colonyParameters.find(x => x.name == 'Economic_Reserves')!.value ?? 0) 
                < -(decree.parameters.find(x => x.name == 'Economic_Reserves')?.value ?? 0))
            return { isActive: false, buttonName: 'Недостаточно солар' }

        if ((myColonyResult.data.data.colonyParameters.find(x => x.name == 'AreaCapacity_Total')!.value ?? 0)
                - (myColonyResult.data.data.colonyParameters.find(x => x.name == 'AreaCapacity_Occupied')!.value ?? 0) 
                < -(decree.parameters.find(x => x.name == 'AreaCapacity_Occupied')?.value ?? 0))
            return { isActive: false, buttonName: 'Недостаточно секторов' }

        return { isActive: true, buttonName: 'Издать указ' }
    }

    const renderCard = (decree: DecreeDetails) => {
        const { isActive, buttonName } = validateDecree(decree);
        return (
            <YagoCard
                title='Указ'
                image={`/assets/images/pictures/${decree.image}.jpg`}
            >
                <YagoCardContentSelection handlePrev={handlePrevDecree} label={decree.name} handleNext={handleNextDecree} />
                <TextMain textArray={decree.text} sx={{ textAlign: 'justify' }} />
                <StateList items={GetStateItems(decree.parameters, true)} />
                <YagoButton onClick={() => navigate(-1)} text={'Закрыть'} isDisabled={false} />
                <YagoButton variant="contained" onClick={() => handleIssueDecree(decree.id)} text={buttonName} isDisabled={!isActive} />
                <YagoButton onClick={() => setShowSlide(true)} text={'Описание'} />
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading || decreeResult.data == undefined
                ? <LoadingCard />
                : error != undefined
                    ? <DefaultErrorCard />
                    : showSlide
                        ? renderSlideCard(decreeResult.data)
                        : renderCard(decreeResult.data)}
        </>
    )
}

export default DecreePage