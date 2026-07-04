import React from 'react';
import { ArrowLeft, Construction } from 'lucide-react';
import YagoCard from '../shared/YagoCard';
import YagoDivider from '../shared/YagoDivider';
import PageContainer from '../shared/PageContainer';
import AnimatedIcon from '../shared/AnimatedIcon';
import YagoTitle from '../shared/YagoTitle';
import YagoText from '../shared/YagoText';
import YagoButton from '../shared/YagoButton';
import { useNavigate } from 'react-router-dom';

const UnderDevelopment: React.FC = () => {
    const navigate = useNavigate();

    const renderIcon = () => (
        <AnimatedIcon
            icon={Construction}
            color="bright"
            size="md"
            pingOpacity={0.2}
        />
    );

    const renderSubtitle = () => (
        <YagoText variant="secondary">
            Эта страница ещё создаётся.
            <br />
            Скоро здесь появится что-то интересное.
        </YagoText>
    );

    const renderActions = () => (
        <div className="flex flex-col items-center gap-3 w-full max-w-xs">
            <YagoButton
                variant="secondary"
                size="sm"
                icon={ArrowLeft}
                onClick={() => navigate(-1)}
            >
                Назад
            </YagoButton>
        </div>
    );

    return (
        <PageContainer backgroundImage='grayСorridor' darkenBackground={false}>
            <YagoCard variant="glow">
                {renderIcon()}
                <YagoTitle>В разработке</YagoTitle>
                {renderSubtitle()}
                {renderActions()}
            </YagoCard>
            <YagoDivider />
        </PageContainer>
    );
};

export default UnderDevelopment;