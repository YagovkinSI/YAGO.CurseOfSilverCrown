import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useEffect } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetQuery } from '../entities/MyUser';
import { useGetMyColonyQuery } from '../entities/MyColony';
import YagoButton from '../shared/YagoButton';
import { useDeactivateColonyMutation } from '../entities/ColonyActions';

const DevelopingPage: React.FC = () => {
    const myUserDataResult = useGetQuery();
    const myColonyResult = useGetMyColonyQuery();

    const [deactivateColony] = useDeactivateColonyMutation();

    const isLoading = myUserDataResult.isLoading || myColonyResult.isLoading;
    const error = myUserDataResult.error ?? myColonyResult.error;

    const navigate = useNavigate();

    useEffect(() => {
        if (!myUserDataResult?.data?.isAuthorized) {
            navigate('/registration');
        }
    }, [myUserDataResult, navigate]);

    useEffect(() => {
        if (myColonyResult.data != undefined && myColonyResult.data!.isAuthorized && myColonyResult.data!.data == undefined) {
            navigate('/createColony');
        }
    }, [navigate, myColonyResult]);

    const deactivateColonyHandle = async () => {
        await deactivateColony({});
        navigate('/me/colony');
    }

    const renderCard = () => {
        return (
            <YagoCard
                title={`Создать новую колонию`}
                image={`/assets/images/pictures/register_colony.jpg`}
            >
                <Typography textAlign="justify" gutterBottom>
                    При создании новой колонии игра начнётся с самого начала.
                    Ваша текущая колония будет сохранена за вами, но в текущей версии у вас не будет к ней доступа
                    и колония перестанет отображаться в списке колоний.
                    Возможно в будущих версиях вы сможете увидеть эту колонию и даже вернуть над ней контроль.
                </Typography>
                <YagoButton variant='contained' color='error' onClick={() => deactivateColonyHandle()} text='Новая колония' />
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

export default DevelopingPage