import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Sparkles, LogIn, Rocket } from 'lucide-react';
import { useCreateTemporaryUserMutation, useGetMyUserQuery } from '../entities/MyUser';
import PageContainer from '../shared/PageContainer';
import YagoCard from '../shared/YagoCard';
import IconAnimated from '../shared/IconAnimated';
import YagoTitle from '../shared/YagoTitle';
import YagoButton from '../shared/YagoButton';
import YagoText from '../shared/YagoText';

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
        navigate('/me');
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
        <YagoText variant="secondary">
            Каким будет твоё государство среди звёзд?
        </YagoText>
    );

    const renderButtons = () => (
        <div className="flex flex-col gap-3 w-full max-w-xs">
            <YagoButton onClick={handleQuickStart} disabled={isLoading} icon={Rocket}>
                {isLoading ? 'Загрузка...' : 'Быстрый старт'}
            </YagoButton>
            <YagoButton variant="secondary" onClick={handleLogin} disabled={isLoading} icon={LogIn}>
                Войти / Регистрация
            </YagoButton>
        </div>
    );

    const renderFooter = () => (
        <YagoText variant="dim" size='xs'>
            Для создания визуального и текстового контента в этой игре в качестве инструмента прототипирования и вдохновения использовались технологии искусственного интеллекта. Финальный творческий отбор и интеграция выполнены разработчиком. Мы с уважением относимся к творчеству художников и писателей по всему миру.
        </YagoText>
    );

    const renderContent = () => {
        return (
            <YagoCard variant="glow" className="flex flex-col items-center gap-6">
                {renderIcon()}
                <YagoTitle>Мир YAGO</YagoTitle>
                {renderSubtitle()}
                {renderButtons()}
                {renderFooter()}
            </YagoCard>
        );
    };

    return (
        <PageContainer backgroundImage='city_in_space' isLoading={isLoading} error={error}>
            {renderContent()}
        </PageContainer>
    );
};

export default HomePage;