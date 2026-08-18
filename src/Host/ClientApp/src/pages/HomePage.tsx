import React from 'react';
import { useNavigate } from 'react-router-dom';
import { Sparkles, Rocket } from 'lucide-react';
import { useCreateTemporaryUserMutation, useGetUserPrivateQuery } from "../entities/users/user.api";
import Card from '../shared/ui/Card';
import IconAnimated from '../shared/ui/IconAnimated';
import Title from '../shared/ui/Title';
import Button from '../shared/ui/buttons/Button';
import Text from '../shared/ui/Text';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/ui/FlexContainer';
import ButtonLink from '../shared/ui/buttons/ButtonLink';

const HomePage: React.FC = () => {
    const navigate = useNavigate();
    
    const getUserPrivateResult = useGetUserPrivateQuery();
    const [createTemporaryUser, createTemporaryUserResult] = useCreateTemporaryUserMutation();

    const user = getUserPrivateResult.data?.data;

    const isPageLoading = getUserPrivateResult.isLoading;
    const error = getUserPrivateResult.error ?? createTemporaryUserResult.error;

    const handlePlay = async () => {
        if (user == undefined)
            await createTemporaryUser().unwrap();
        navigate('/me/colony');
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

    const renderSubtitle = () => {
        const text = user == undefined
            ? 'Каким будет твоё государство среди звёзд?'
            : `Приветсвую, ${user.userName}!`
        return <Text variant="secondary" size="lg">
            {text}
        </Text>
    };

    const renderLoginLink = () => (
        <ButtonLink
            variant='secondary' disabled={isPageLoading} onClick={() => navigate('/registration')}
        >
            Уже есть аккаунт? Войти
        </ButtonLink>
    )

    const renderConvertToPermanentLink = () => (
        <ButtonLink
            variant='secondary' disabled={isPageLoading} onClick={() => navigate('/user/convertToPermanent')}
        >
            Перевести аккаунт в постоянный
        </ButtonLink>
    )

    const renderButtons = () => {
        const isLoading = isPageLoading || createTemporaryUserResult.isLoading;
        return <div className="flex flex-col gap-4 w-full mt-2">
            <Button 
                onClick={handlePlay} 
                disabled={isLoading}
                icon={Rocket} 
                iconPosition="left"
            >
                {isLoading 
                    ? 'Загрузка...' 
                    : user == undefined 
                        ? 'Начать игру'
                        : 'Играть'}
            </Button>
            {!user && renderLoginLink()}
            {user?.isTemporary && renderConvertToPermanentLink()}
        </div>
    };

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
        <Page backgroundImage='city_in_space' isLoading={isPageLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default HomePage;
