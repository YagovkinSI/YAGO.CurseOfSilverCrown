import React, { useEffect } from 'react';
import { Sparkles } from 'lucide-react';
import Card from '../shared/ui/Card';
import Divider from '../shared/ui/Divider';
import IconAnimated from '../shared/ui/IconAnimated';
import Title from '../shared/ui/Title';
import { useNavigate } from 'react-router-dom';
import Page from '../widgets/Page';
import { useCreateColonyMutation } from '../entities/colonies/colony.api';

const CreateColonyPage: React.FC = () => {
    const navigate = useNavigate();

    const [createColony, createColonyResult] = useCreateColonyMutation();

    const isLoading = createColonyResult.isLoading;
    const error = createColonyResult.error;

    useEffect(() => {
            const fetchResult = async () => {
                const result = await createColony().unwrap();
                if (result.data) {
                    const autostartEvent = result.data.quests.find(x => x.type == 'Autostart');
                    if (autostartEvent)
                        navigate(`/me/events/${autostartEvent.id}`);
                    else
                        navigate('/me/colony');
                }
            };
            fetchResult();
        }, [createColony, navigate]);

    const renderIcon = () => (
        <IconAnimated
            icon={Sparkles}
            color="bright"
            size="xl"
            pingOpacity={0.3}
            className="md:scale-110"
        />
    );

    const renderContent = () => (
        <div className="flex flex-l items-center justify-center w-full min-h-full py-2">
            <Card variant="glow">
                {renderIcon()}
                <Title>Создание колонии...</Title>
            </Card>
            <Divider />
        </div>
    );

    return (
        <Page backgroundImage={undefined} isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default CreateColonyPage;