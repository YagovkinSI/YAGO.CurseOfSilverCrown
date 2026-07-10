import SlideCard from '../shared/SlideCard';
import { useEffect } from 'react';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetMyUserQuery } from '../entities/MyUser';
import { useDeactivateColonyMutation, useGetMyColonyQuery } from '../entities/MyColony';
import Button from '../shared/Button';
import PageContainer from '../widgets/ContainerPage';

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
    };

    const renderDescription = () => (
        <p className="text-justify text-light/90 text-base mb-4">
            При создании новой колонии игра начнётся с самого начала.
            Ваша текущая колония будет сохранена за вами, но в текущей версии у вас не будет к ней доступа
            и колония перестанет отображаться в списке колоний.
            Возможно в будущих версиях вы сможете увидеть эту колонию и даже вернуть над ней контроль.
        </p>
    );

    const renderContent = () => (
        <SlideCard
            title="Создать новую колонию"
            image="/assets/images/pictures/register_colony.jpg"
        >
            <div className="flex flex-col gap-4 items-center">
                {renderDescription()}
                <Button onClick={deactivateColonyHandle} variant="danger">
                    Новая колония
                </Button>
            </div>
        </SlideCard>
    );

    return (
        <PageContainer backgroundImage='space' isLoading={isLoading} error={error}>
            {renderContent()}
        </PageContainer>
    );
};

export default DevelopingPage;