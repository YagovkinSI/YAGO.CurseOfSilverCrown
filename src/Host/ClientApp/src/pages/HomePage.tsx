import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Sparkles, LogIn, Rocket } from 'lucide-react';
import { useCreateTemporaryUserMutation, useGetMyUserQuery } from '../entities/MyUser';
import Card from '../shared/Card';
import IconAnimated from '../shared/IconAnimated';
import Title from '../shared/Title';
import Button from '../shared/Button';
import Text from '../shared/Text';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/FlexContainer';

const HomePage: React.FC = () => {
    const navigate = useNavigate();
    const getMyUserResult = useGetMyUserQuery();
    const [createTemporaryUser, createTemporaryUserResult] = useCreateTemporaryUserMutation();

    const isLoading = getMyUserResult.isLoading || createTemporaryUserResult.isLoading;
    const error = getMyUserResult.error ?? createTemporaryUserResult.error;

    const user = getMyUserResult.data?.data;

    React.useEffect(() => {
        if (!getMyUserResult.isFetching && !isLoading && user) {
            navigate('/me/colony');
        }
    }, [user, isLoading, navigate]);

    const handleQuickStart = async () => {
        await createTemporaryUser().unwrap();
        navigate('/me/colony');
    };

    const handleLogin = () => {
        navigate('/registration');
    };

    const renderIcon = () => (
        <IconAnimated
            icon={Sparkles}
            color="bright"
            size="xl"
            pingOpacity={0.3}
            className="md:scale-110"
        />
    );

    const renderSubtitle = () => (
        <Text variant="secondary" size="lg">
            Каким будет твоё государство среди звёзд?
        </Text>
    );

    const renderButtons = () => (
        <div className="flex flex-col gap-4 w-full mt-2">
            <Button onClick={handleQuickStart} disabled={isLoading} 
                icon={Rocket} iconPosition="left"
            >
                {isLoading ? 'Загрузка...' : 'Быстрый старт'}
            </Button>
            
            <Button 
                variant="secondary" onClick={handleLogin} disabled={isLoading} 
                icon={LogIn} iconPosition="left" uppercase={false}
            >
                Войти / Регистрация
            </Button>
        </div>
    );

    const renderFooter = () => (
        <Text variant="glass-dim" size="xs" maxWidth="md" 
            className="text-center mt-2"
        >
            Контент создан с использованием ИИ в качестве инструмента прототипирования и вдохновения. Все финальные решения приняты разработчиком.
        </Text>
    );

    const renderContent = () => {
        return (
            <div className='h-full overflow-y-auto scrollbar-hide'>
                <FlexContainer className='p-2'>
                    <Card variant="glow" className="w-full flex flex-col items-center">
                        {renderIcon()}
                        <Title uppercase={false} size="h1">Мир YAGO</Title>
                        {renderSubtitle()}
                        {renderButtons()}
                        {renderFooter()}
                    </Card>
                </FlexContainer>
            </div>
        );
    };

    return (
        <Page backgroundImage='city_in_space' isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default HomePage;