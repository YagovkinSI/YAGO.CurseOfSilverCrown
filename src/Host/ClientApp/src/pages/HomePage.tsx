import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Sparkles, LogIn, Rocket } from 'lucide-react';
import { useCreateTemporaryUserMutation, useGetMyUserQuery } from '../entities/MyUser';
import PageContainer from '../widgets/ContainerPage';
import Card from '../shared/Card';
import IconAnimated from '../shared/IconAnimated';
import Title from '../shared/Title';
import Button from '../shared/Button';
import Text from '../shared/Text';

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
            pingOpacity={0.2}
            className="md:scale-110"
        />
    );

    const renderSubtitle = () => (
        <Text variant="secondary">
            Каким будет твоё государство среди звёзд?
        </Text>
    );

    const renderButtons = () => (
        <div className="flex flex-col gap-3 w-full max-w-xs">
            <Button onClick={handleQuickStart} disabled={isLoading} icon={Rocket}>
                {isLoading ? 'Загрузка...' : 'Быстрый старт'}
            </Button>
            <Button variant="secondary" onClick={handleLogin} disabled={isLoading} icon={LogIn}>
                Войти / Регистрация
            </Button>
        </div>
    );

    const renderFooter = () => (
        <Text variant="dim" size='xs'>
            Для создания визуального и текстового контента в этой игре в качестве инструмента прототипирования и вдохновения использовались технологии искусственного интеллекта. Финальный творческий отбор и интеграция выполнены разработчиком. Мы с уважением относимся к творчеству художников и писателей по всему миру.
        </Text>
    );

    const renderContent = () => {
        return (
            <Card variant="glow" className="flex flex-col items-center gap-6">
                {renderIcon()}
                <Title>Мир YAGO</Title>
                {renderSubtitle()}
                {renderButtons()}
                {renderFooter()}
            </Card>
        );
    };

    return (
        <PageContainer backgroundImage='city_in_space' isLoading={isLoading} error={error}>
            {renderContent()}
        </PageContainer>
    );
};

export default HomePage;