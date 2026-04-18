import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Typography } from '@mui/material';
import { useEffect } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetMyUserQuery } from '../entities/MyUser';
import { useDeactivateColonyMutation, useGetMyColonyQuery } from '../entities/MyColony';
import YagoButton from '../shared/YagoButton';

const DevelopingPage: React.FC = () => {
    const myUserDataResult = useGetMyUserQuery();
    const myColonyResult = useGetMyColonyQuery();
    
    const [deactivateColony] = useDeactivateColonyMutation();

    const isLoading = myUserDataResult.isLoading || myColonyResult.isLoading;
    const error = myUserDataResult.error ?? myColonyResult.error;

    const navigate = useNavigate();

    useEffect(() => {
        if (!(myUserDataResult?.data?.data != undefined)) {
            navigate('/registration');
        }
    }, [myUserDataResult, navigate]);

    useEffect(() => {
        if (myUserDataResult?.data?.data != undefined && myColonyResult.data != undefined && myColonyResult.data!.data == undefined) {
            navigate('/me/colony');
        }
    }, [navigate, myUserDataResult, myColonyResult]);

    const deactivateColonyHandle = async () => {
        await deactivateColony();
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
                <YagoButton onClick={() => deactivateColonyHandle()} type='delete-confirm'>Новая колония</YagoButton>
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