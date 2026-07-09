import YagoSlide from '../shared/YagoSlide';
import YagoButton from '../shared/YagoButton';
import { useNavigate } from 'react-router-dom';
import { useCreateTemporaryUserMutation, useGetMyUserQuery, type MyUser } from '../entities/MyUser';
import TextFooterComment from '../shared/TextFooterComment';
import { useGetMyColonyQuery } from '../entities/MyColony';
import PageContainer from '../shared/PageContainer';

const HomePage: React.FC = () => {
    const getMyUserResult = useGetMyUserQuery();
    const getMyColonyResult = useGetMyColonyQuery();
    const [createTemporaryUser, createTemporaryUserResult] = useCreateTemporaryUserMutation();
    const navigate = useNavigate();

    const isLoading = getMyUserResult.isLoading || getMyColonyResult.isLoading || createTemporaryUserResult.isLoading;
    const error = getMyUserResult.error ?? getMyColonyResult.error ?? createTemporaryUserResult.error;

    const user = getMyUserResult.data?.data;
    const colony = getMyColonyResult.data?.data;

    const autoRegisterAndGame = async () => {
        await createTemporaryUser().unwrap();
        navigate('/me/colony');
    };

    const renderGuestContent = () => (
        <div className="flex flex-col gap-3 items-center">
            <YagoButton onClick={autoRegisterAndGame}>Быстрый старт</YagoButton>
            <YagoButton onClick={() => navigate('/registration')} variant="secondary">
                Войти / Регистрация
            </YagoButton>
        </div>
    );

    const renderAuthorizedUserContent = (user: MyUser) => {
        const buttonName = colony == undefined
            ? 'Создать колонию'
            : user.isTemporary
                ? `Продолжить как ${user.userName}`
                : 'Игра';
        return (
            <div className="flex flex-col gap-3 items-center">
                {user.isTemporary && (
                    <YagoButton 
                        onClick={() => navigate('/registration')} 
                        variant="secondary"
                    >
                        Изменить имя и пароль
                    </YagoButton>
                )}
                <YagoButton onClick={() => navigate('/me/colony')}>
                    {buttonName}
                </YagoButton>
            </div>
        );
    };

    const renderDescription = () => (
        <p className="text-center text-light/80 text-base mb-4">
            Каким будет твоё государство среди звёзд?
        </p>
    );

    const renderContent = () => (
        <YagoSlide
            title="Мир YAGO"
            image="/assets/images/pictures/homepage.jpg"
            headerButtonsAccess={false}
        >
            <div className="flex flex-col gap-4">
                {renderDescription()}
                {user != undefined
                    ? renderAuthorizedUserContent(user)
                    : renderGuestContent()}
                <TextFooterComment>
                    Для создания визуального и текстового контента в этой игре в качестве инструмента прототипирования и вдохновения использовались технологии искусственного интеллекта. Финальный творческий отбор и интеграция выполнены разработчиком. Мы с уважением относимся к творчеству художников и писателей по всему миру.
                </TextFooterComment>
            </div>
        </YagoSlide>
    );

    return (
        <PageContainer backgroundImage='homepage' isLoading={isLoading} error={error}>
            {renderContent()}
        </PageContainer>
    );
};

export default HomePage;