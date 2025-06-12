import YagoCard from '../shared/YagoCard';
import YagoLogo from '../features/YagoLogo';
import type YagoLink from '../entities/YagoLink';

const HomePage: React.FC = () => {
  const title: YagoLink = { name: "Yago World" }

  return (
    <YagoCard title={title}>
      <div>
        <YagoLogo />
      </div>
    </YagoCard>
  )
}

export default HomePage