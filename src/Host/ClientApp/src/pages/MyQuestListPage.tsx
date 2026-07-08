import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { useEffect } from 'react';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetMyUserQuery } from '../entities/MyUser';
import YagoButton from '../shared/YagoButton';
import RowData from '../shared/RowData';
import { AlertCircle } from 'lucide-react';
import { useGetMyColonyQuery } from '../entities/MyColony';
import { GetColorForQuestType, type MyQuest } from '../entities/MyQuest';

const MyQuestListPage: React.FC = () => {
    const navigate = useNavigate();
    const myUserDataResult = useGetMyUserQuery();
    const myColonyResult = useGetMyColonyQuery();

    const isLoading = myUserDataResult.isLoading || myColonyResult.isLoading;
    const error = myUserDataResult.error ?? myColonyResult.error;

    useEffect(() => {
        if (!(myUserDataResult.data?.data != undefined)) {
            navigate('/registration');
        }
    }, [myUserDataResult, navigate]);

    const renderQuest = (quest: MyQuest) => {
        const color = GetColorForQuestType([quest.type]);
        const url = `/me/quest/${quest.id}`;
        return (
            <RowData 
                color={color} 
                icon={AlertCircle} 
                label={quest.title} 
                value={quest.progress} 
                url={url} 
            />
        );
    };

    const renderQuestsList = (quests: MyQuest[]) => (
        <div className="flex flex-col gap-1 w-full max-w-[350px] md:max-w-[700px] mx-auto">
            {quests.map((q, index) => (
                <React.Fragment key={q.id || index}>
                    {renderQuest(q)}
                </React.Fragment>
            ))}
        </div>
    );

    const renderCard = () => {
        const quests = myColonyResult.data!.data!.quests;

        return (
            <YagoCard
                title="События"
                image="/assets/images/pictures/captain_hall.jpg"
            >
                <div className="flex flex-col gap-4 items-center">
                    {renderQuestsList(quests)}
                    <YagoButton onClick={() => navigate(-1)} type="secondary">
                        Закрыть
                    </YagoButton>
                </div>
            </YagoCard>
        );
    };

    const renderContent = () => {
        if (isLoading) {
            return <LoadingCard />;
        }
        if (error != undefined) {
            return <DefaultErrorCard />;
        }
        return renderCard();
    };

    return (
        <>
            <ErrorField title="Ошибка" error={error} />
            {renderContent()}
        </>
    );
};

export default MyQuestListPage;